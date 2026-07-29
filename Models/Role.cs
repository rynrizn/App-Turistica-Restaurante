using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa un rol asignable a los usuarios del sistema.
/// </summary>
public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
