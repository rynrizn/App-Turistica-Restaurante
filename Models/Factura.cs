using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa la factura generada a partir de una reserva
/// </summary>
public partial class Factura
{
    public int Id { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public DateTime? FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Total { get; set; }

    public string EstadoPago { get; set; } = null!;

    public int ReservaId { get; set; }

    public int FormaPagoId { get; set; }

    public virtual FormasPago FormaPago { get; set; } = null!;

    public virtual Reserva Reserva { get; set; } = null!;
}
