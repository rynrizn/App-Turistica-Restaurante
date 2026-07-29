using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa la configuracion general del restaurante
/// </summary>
public partial class ConfiguracionRestaurante
{
    public int Id { get; set; }

    public string Clave { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public string? Descripcion { get; set; }
}
