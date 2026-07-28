using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class MenuItem
{
    public int Id { get; set; }

    public string NombrePantalla { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public string NombreComponente { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public string? RolRequerido { get; set; }
}
