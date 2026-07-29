using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;
/// <summary>
/// Representa la relacion entre una reserva y la mesa asignada.
/// </summary>
public partial class ReservaMesa
{
    public int Id { get; set; }

    public int ReservaId { get; set; }

    public int MesaId { get; set; }

    public virtual Mesa Mesa { get; set; } = null!;

    public virtual Reserva Reserva { get; set; } = null!;
}
