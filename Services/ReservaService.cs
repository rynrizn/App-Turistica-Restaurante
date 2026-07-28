using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;

public class ReservaService
{
    private readonly IDbContextFactory<AppWebContext> _dbFactory;

    public ReservaService(IDbContextFactory<AppWebContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    /// <summary>
    /// Busca mesas con capacidad suficiente que no tengan reservas activas en la misma fecha y hora.
    /// </summary>
    public async Task<List<Mesa>> GetMesasDisponiblesAsync(DateOnly fecha, TimeOnly hora, int cantidadPersonas)
    {
        using var context = _dbFactory.CreateDbContext();

        // IDs de mesas ocupadas en esa fecha+hora con reservas activas
        var mesasOcupadasIds = await context.ReservaMesas
            .Where(rm => rm.Reserva.FechaReserva == fecha
                && rm.Reserva.HoraReserva == hora
                && (rm.Reserva.Estado == "Pendiente" || rm.Reserva.Estado == "Confirmada"))
            .Select(rm => rm.MesaId)
            .ToListAsync();

        return await context.Mesas
            .Where(m => m.Estado == true
                && m.CapacidadPersonas >= cantidadPersonas
                && !mesasOcupadasIds.Contains(m.Id))
            .OrderBy(m => m.CapacidadPersonas)
            .ToListAsync();
    }

    /// <summary>
    /// Crea una reserva completa: genera código único, inserta reserva, asigna mesa, y opcionalmente guarda pre-pedido.
    /// </summary>
    public async Task<string> CrearReservaAsync(int usuarioId, DateOnly fecha, TimeOnly hora, int cantidadPersonas,
        string? observaciones, int mesaId, List<(int productoId, int cantidad, decimal precioUnitario)>? preOrden)
    {
        using var context = _dbFactory.CreateDbContext();

        // Generar código único tipo RES-XXXXXX
        string codigo = $"RES-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        var reserva = new Reserva
        {
            CodigoReserva = codigo,
            FechaReserva = fecha,
            HoraReserva = hora,
            CantidadPersonas = cantidadPersonas,
            Estado = "Pendiente",
            Observaciones = observaciones,
            UsuarioId = usuarioId
        };

        context.Reservas.Add(reserva);
        await context.SaveChangesAsync();

        // Asignar mesa
        context.ReservaMesas.Add(new ReservaMesa
        {
            ReservaId = reserva.Id,
            MesaId = mesaId
        });

        // Pre-pedido opcional
        if (preOrden != null && preOrden.Count > 0)
        {
            foreach (var item in preOrden)
            {
                context.DetalleReservas.Add(new DetalleReserva
                {
                    ReservaId = reserva.Id,
                    ProductoId = item.productoId,
                    Cantidad = item.cantidad,
                    PrecioUnitario = item.precioUnitario
                });
            }
        }

        await context.SaveChangesAsync();
        return codigo;
    }

    /// <summary>
    /// Obtiene las reservas de un usuario con sus mesas y detalles de productos.
    /// </summary>
    public async Task<List<Reserva>> GetReservasByUsuarioAsync(int usuarioId)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Reservas
            .Include(r => r.ReservaMesas).ThenInclude(rm => rm.Mesa)
            .Include(r => r.DetalleReservas).ThenInclude(d => d.Producto)
            .Include(r => r.Factura)
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.FechaReserva)
            .ThenByDescending(r => r.HoraReserva)
            .ToListAsync();
    }

    /// <summary>
    /// Cancela una reserva si faltan al menos 2 horas antes.
    /// </summary>
    public async Task<(bool exito, string mensaje)> CancelarReservaAsync(int reservaId, int usuarioId)
    {
        using var context = _dbFactory.CreateDbContext();
        var reserva = await context.Reservas.FindAsync(reservaId);

        if (reserva == null) return (false, "Reserva no encontrada.");
        if (reserva.UsuarioId != usuarioId) return (false, "No tiene permiso para cancelar esta reserva.");
        if (reserva.Estado != "Pendiente" && reserva.Estado != "Confirmada")
            return (false, "Solo se pueden cancelar reservas Pendientes o Confirmadas.");

        // Validar mínimo 2 horas antes
        var fechaHoraReserva = reserva.FechaReserva.ToDateTime(reserva.HoraReserva);
        if ((fechaHoraReserva - DateTime.Now).TotalHours < 2)
            return (false, "Solo puede cancelar con al menos 2 horas de anticipación.");

        reserva.Estado = "Cancelada";
        reserva.Observaciones = (reserva.Observaciones ?? "") + " | Cancelada por el cliente.";
        await context.SaveChangesAsync();
        return (true, "Reserva cancelada exitosamente.");
    }

    /// <summary>
    /// Obtiene todas las reservas (para empleados/admin) con datos completos.
    /// </summary>
    public async Task<List<Reserva>> GetTodasReservasAsync(DateOnly? fechaFiltro = null)
    {
        using var context = _dbFactory.CreateDbContext();
        var query = context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.ReservaMesas).ThenInclude(rm => rm.Mesa)
            .Include(r => r.DetalleReservas).ThenInclude(d => d.Producto)
            .Include(r => r.Factura)
            .AsQueryable();

        if (fechaFiltro.HasValue)
            query = query.Where(r => r.FechaReserva == fechaFiltro.Value);

        return await query
            .OrderByDescending(r => r.FechaReserva)
            .ThenByDescending(r => r.HoraReserva)
            .ToListAsync();
    }

    /// <summary>
    /// Actualiza el estado de una reserva (empleado/admin).
    /// </summary>
    public async Task<bool> ActualizarEstadoReservaAsync(int reservaId, string nuevoEstado, DateTime? fechaEntrada = null, DateTime? fechaSalida = null)
    {
        using var context = _dbFactory.CreateDbContext();
        var reserva = await context.Reservas.FindAsync(reservaId);
        if (reserva == null) return false;

        reserva.Estado = nuevoEstado;
        if (fechaEntrada.HasValue) reserva.FechaEntrada = fechaEntrada;
        if (fechaSalida.HasValue) reserva.FechaSalida = fechaSalida;

        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Genera una factura para una reserva completada.
    /// </summary>
    public async Task<Factura?> GenerarFacturaAsync(int reservaId, int formaPagoId, decimal descuento = 0)
    {
        using var context = _dbFactory.CreateDbContext();
        var reserva = await context.Reservas
            .Include(r => r.DetalleReservas)
            .Include(r => r.Factura)
            .FirstOrDefaultAsync(r => r.Id == reservaId);

        if (reserva == null || reserva.Factura != null) return null;

        decimal subtotal = reserva.DetalleReservas.Sum(d => d.Cantidad * d.PrecioUnitario);
        decimal total = subtotal - descuento;
        if (total < 0) total = 0;

        // Generar número de factura secuencial
        int ultimoId = await context.Facturas.CountAsync();
        string numeroFactura = $"FAC-{DateTime.Now.Year}-{(ultimoId + 1):D4}";

        var factura = new Factura
        {
            NumeroFactura = numeroFactura,
            Subtotal = subtotal,
            Descuento = descuento,
            Total = total,
            EstadoPago = "Pendiente",
            ReservaId = reservaId,
            FormaPagoId = formaPagoId
        };

        context.Facturas.Add(factura);
        await context.SaveChangesAsync();
        return factura;
    }

    /// <summary>
    /// Marca una factura como pagada.
    /// </summary>
    public async Task<bool> CompletarPagoAsync(int facturaId)
    {
        using var context = _dbFactory.CreateDbContext();
        var factura = await context.Facturas.FindAsync(facturaId);
        if (factura == null) return false;

        factura.EstadoPago = "Pagado";
        factura.FechaEmision = DateTime.Now;
        return await context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Obtiene las formas de pago activas.
    /// </summary>
    public async Task<List<FormasPago>> GetFormasPagoAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.FormasPagos
            .Where(f => f.Estado == true)
            .OrderBy(f => f.Nombre)
            .ToListAsync();
    }
}
