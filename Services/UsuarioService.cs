using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;
/// <summary>
/// Gestiona las operaciones relacionadas con los usuarios.
/// </summary>
public class UsuarioService
{
    private readonly IDbContextFactory<AppWebContext> _dbFactory;

    public UsuarioService(IDbContextFactory<AppWebContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    // 1. Obtener listado de usuarios Activos (Filtro de Baja Lógica)
    public async Task<List<Usuario>> GetUsuariosActivosAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Usuarios
            .Include(u => u.Rol)
            .Where(u => u.Estado == true) // Filtro del lado del servidor
            .OrderBy(u => u.Apellidos)
            .ThenBy(u => u.Nombres)
            .ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // 2. Obtener lista de roles disponibles
    public async Task<List<Role>> GetRolesAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Roles.OrderBy(r => r.Nombre).ToListAsync();
    }

    // 3. Registrar un nuevo usuario
    public async Task<bool> InsertUsuarioAsync(Usuario usuario, string passwordPlana)
    {
        using var context = _dbFactory.CreateDbContext();
        bool existe = await context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
        if (existe) return false;

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlana);
        usuario.Estado = true;

        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return true;
    }

    // 4. Modificación Segura (Mapeo Selectivo)
    public async Task<bool> ActualizarUsuarioAsync(Usuario usuarioModificado)
    {
        using var context = _dbFactory.CreateDbContext();
        var usuarioDb = await context.Usuarios.FindAsync(usuarioModificado.Id);
        
        if (usuarioDb == null) return false;

        // Solo se alteran las propiedades autorizadas
        usuarioDb.Nombres = usuarioModificado.Nombres;
        usuarioDb.Apellidos = usuarioModificado.Apellidos;
        usuarioDb.Telefono = usuarioModificado.Telefono;
        usuarioDb.RolId = usuarioModificado.RolId;
        // Nota: El email y password no se editan desde aquí por seguridad.

        context.Usuarios.Update(usuarioDb);
        return await context.SaveChangesAsync() > 0;
    }

    // 5. Baja Lógica (Soft Delete)
    public async Task<bool> DarDeBajaAsync(int id)
    {
        using var context = _dbFactory.CreateDbContext();
        var usuarioDb = await context.Usuarios.FindAsync(id);
        
        if (usuarioDb == null) return false;

        // Preserva la integridad referencial histórica cambiando solo el estado
        usuarioDb.Estado = false; 
        context.Usuarios.Update(usuarioDb);
        
        return await context.SaveChangesAsync() > 0;
    }
}