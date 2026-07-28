using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class VHistorialRestaurante
{
    public int? FacturaId { get; set; }

    public string? NumeroFactura { get; set; }

    public DateTime? FechaEmision { get; set; }

    public string? CodigoReserva { get; set; }

    public string? Cliente { get; set; }

    public string? ClienteEmail { get; set; }

    public int? NumeroMesa { get; set; }

    public string? Producto { get; set; }

    public string? CategoriaProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? SubtotalLinea { get; set; }

    public string? MetodoPago { get; set; }

    public decimal? TotalFactura { get; set; }

    public DateTime? FechaEntrada { get; set; }

    public DateTime? FechaSalida { get; set; }

    public TimeSpan? TiempoEstadia { get; set; }
}
