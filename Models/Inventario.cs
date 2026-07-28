using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class Inventario
{
    public int Id { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public bool Estado { get; set; }

    public int ProductoId { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
