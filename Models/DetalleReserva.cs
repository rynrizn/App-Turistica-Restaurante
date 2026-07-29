using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
 /// <summary>
/// Clase que nos sirve para almacenar los productos solicitados dentro de una reserva
/// </summary>
public partial class DetalleReserva
{
    public int Id { get; set; }

    public int ReservaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Reserva Reserva { get; set; } = null!;
}
