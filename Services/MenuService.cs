using Microsoft.EntityFrameworkCore;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services;
/// <summary>
/// Gestiona las operaciones relacionadas con el menú.
/// </summary>
public class MenuService
{
    private readonly AppWebContext _context;

    public MenuService(AppWebContext context)
    {
        _context = context;
    }

    public async Task<List<MenuItem>> ObtenerMenuItemsAsync()
    {
        return await _context.MenuItems
            .OrderBy(x => x.Id)
            .ToListAsync();
    }
}