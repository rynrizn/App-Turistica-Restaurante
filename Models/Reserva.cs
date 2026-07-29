using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa una reserva realizada por un usuario en el restaurante
/// </summary>
public partial class Reserva
{
    public int Id { get; set; }

    public string CodigoReserva { get; set; } = null!;

    public DateOnly FechaReserva { get; set; }

    public TimeOnly HoraReserva { get; set; }

    public DateTime? FechaEntrada { get; set; }

    public DateTime? FechaSalida { get; set; }

    public int CantidadPersonas { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<DetalleReserva> DetalleReservas { get; set; } = new List<DetalleReserva>();

    public virtual Factura? Factura { get; set; }

    public virtual ICollection<ReservaMesa> ReservaMesas { get; set; } = new List<ReservaMesa>();

    public virtual Usuario Usuario { get; set; } = null!;
}
