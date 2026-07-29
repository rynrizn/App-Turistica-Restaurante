using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;
/// <summary>
/// Gestiona las operaciones relacionadas con los productos.
/// </summary>
public class ProductoService
{
    private readonly IDbContextFactory<AppWebContext> _dbFactory;

    public ProductoService(IDbContextFactory<AppWebContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Producto>> GetProductosConCategoriaAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Productos
            .Include(p => p.Categoria)
            .OrderBy(p => p.Categoria.Nombre)
            .ThenBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<List<Producto>> GetProductosDisponiblesAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Disponible == true && p.Categoria.Estado == true)
            .OrderBy(p => p.Categoria.Nombre)
            .ThenBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<List<Categoria>> GetCategoriasActivasAsync()
    {
        using var context = _dbFactory.CreateDbContext();
        return await context.Categorias
            .Where(c => c.Estado == true)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<bool> CrearProductoAsync(Producto producto)
    {
        using var context = _dbFactory.CreateDbContext();
        context.Productos.Add(producto);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ActualizarProductoAsync(Producto producto)
    {
        using var context = _dbFactory.CreateDbContext();
        var existente = await context.Productos.FindAsync(producto.Id);
        if (existente == null) return false;

        existente.Nombre = producto.Nombre;
        existente.Descripcion = producto.Descripcion;
        existente.Precio = producto.Precio;
        existente.ImagenUrl = producto.ImagenUrl;
        existente.Disponible = producto.Disponible;
        existente.CategoriaId = producto.CategoriaId;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ToggleDisponibilidadAsync(int productoId)
    {
        using var context = _dbFactory.CreateDbContext();
        var producto = await context.Productos.FindAsync(productoId);
        if (producto == null) return false;

        producto.Disponible = !(producto.Disponible ?? true);
        return await context.SaveChangesAsync() > 0;
    }
}
