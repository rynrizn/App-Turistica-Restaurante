using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class FormasPago
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
