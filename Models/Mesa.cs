using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class Mesa
{
    public int Id { get; set; }

    public int NumeroMesa { get; set; }

    public int CapacidadPersonas { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<ReservaMesa> ReservaMesas { get; set; } = new List<ReservaMesa>();
}
