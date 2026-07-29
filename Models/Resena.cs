using System;
using System.Collections.Generic;

namespace RestauranteTuristicoApp.Models;

public partial class Resena
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int Calificacion { get; set; }

    public string Comentario { get; set; } = null!;

    public DateTime FechaResena { get; set; }

    public bool Estado { get; set; }

    public virtual Usuario? Usuario { get; set; } = null!;
}
