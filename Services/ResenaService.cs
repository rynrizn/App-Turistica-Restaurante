using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;
/// <summary>
/// Gestiona las operaciones relacionadas con las reseñas.
/// </summary>
public class ResenaService
{
    private readonly IDbContextFactory<AppWebContext> _dbFactory;

    public ResenaService(IDbContextFactory<AppWebContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Resena>> GetResenasPublicasAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Resenas
            .Include(r => r.Usuario)
            .Where(r => r.Estado == true)
            .OrderByDescending(r => r.FechaResena)
            .ToListAsync();
    }

    public async Task<List<Resena>> GetTodasResenasAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Resenas
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaResena)
            .ToListAsync();
    }

    public async Task<List<Resena>> GetResenasPorUsuarioAsync(int usuarioId)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Resenas
            .Include(r => r.Usuario)
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.FechaResena)
            .ToListAsync();
    }

    public async Task<bool> CrearResenaAsync(Resena resena)
    {
        using var context = _dbFactory.CreateDbContext();
        resena.FechaResena = DateTime.Now;
        resena.Estado = true;
        context.Resenas.Add(resena);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleEstadoResenaAsync(int resenaId)
    {
        using var context = _dbFactory.CreateDbContext();
        var resena = await context.Resenas.FindAsync(resenaId);
        if (resena == null) return false;

        resena.Estado = !resena.Estado;
        await context.SaveChangesAsync();
        return true;
    }
}
