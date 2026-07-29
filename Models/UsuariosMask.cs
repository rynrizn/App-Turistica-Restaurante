using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa una vista simplificada de los datos de un usuario.
/// </summary>
public partial class UsuariosMask
{
    public int? Id { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }
}
