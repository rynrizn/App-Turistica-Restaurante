using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool Estado { get; set; }

    public int RolId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Role Rol { get; set; } = null!;

    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    public Usuario Clonar()
    {
        // Realiza una copia superficial aislando el objeto del listado original
        return (Usuario)this.MemberwiseClone(); 
    }
}
