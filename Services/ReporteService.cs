using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;

public class ResumenDashboard
{
    public int ReservasHoy { get; set; }
    public int ReservasPendientes { get; set; }
    public int ReservasCompletadasMes { get; set; }
    public decimal TotalFacturadoMes { get; set; }
    public List<TopPlatilloItem> TopPlatillos { get; set; } = new();
    public List<Reserva> UltimasReservas { get; set; } = new();
    public int MesasActivas { get; set; }
}

public class TopPlatilloItem
{
    public string NombreProducto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int CantidadTotal { get; set; }
}

public class ReporteService
{
    private readonly IDbContextFactory<AppWebContext> _dbFactory;

    public ReporteService(IDbContextFactory<AppWebContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<ResumenDashboard> GetResumenDashboardAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        var reservasHoy = await context.Reservas.CountAsync(r => r.FechaReserva == hoy);
        var reservasPendientes = await context.Reservas.CountAsync(r => r.Estado == "Pendiente");
        
        var reservasCompletadasMes = await context.Reservas
            .CountAsync(r => r.Estado == "Completada" && r.FechaReserva >= DateOnly.FromDateTime(inicioMes));

        var totalFacturadoMes = await context.Facturas
            .Where(f => f.FechaEmision >= inicioMes && f.EstadoPago == "Pagado")
            .SumAsync(f => (decimal?)f.Total) ?? 0m;

        // Top platillos más solicitados en detalle_reservas
        var topPlatillosQuery = await context.DetalleReservas
            .Include(d => d.Producto).ThenInclude(p => p.Categoria)
            .GroupBy(d => new { d.Producto.Nombre, CategoriaNombre = d.Producto.Categoria.Nombre })
            .Select(g => new TopPlatilloItem
            {
                NombreProducto = g.Key.Nombre,
                Categoria = g.Key.CategoriaNombre,
                CantidadTotal = g.Sum(d => d.Cantidad)
            })
            .OrderByDescending(t => t.CantidadTotal)
            .Take(5)
            .ToListAsync();

        var ultimasReservas = await context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.ReservaMesas).ThenInclude(rm => rm.Mesa)
            .OrderByDescending(r => r.Id)
            .Take(6)
            .ToListAsync();

        var mesasActivas = await context.Mesas.CountAsync(m => m.Estado == true);

        return new ResumenDashboard
        {
            ReservasHoy = reservasHoy,
            ReservasPendientes = reservasPendientes,
            ReservasCompletadasMes = reservasCompletadasMes,
            TotalFacturadoMes = totalFacturadoMes,
            TopPlatillos = topPlatillosQuery,
            UltimasReservas = ultimasReservas,
            MesasActivas = mesasActivas
        };
    }
}
