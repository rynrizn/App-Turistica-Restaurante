using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Almacena informacion sobre cada mesa del restaurante
/// </summary>
public partial class Mesa
{
    public int Id { get; set; }

    public int NumeroMesa { get; set; }

    public int CapacidadPersonas { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<ReservaMesa> ReservaMesas { get; set; } = new List<ReservaMesa>();
}
