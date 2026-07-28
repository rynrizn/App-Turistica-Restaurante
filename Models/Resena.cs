using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteTuristicoApp.Models;

[Table("resenas")]
public partial class Resena
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("calificacion")]
    [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas.")]
    public int Calificacion { get; set; } = 5;

    [Column("comentario")]
    [Required(ErrorMessage = "El comentario de la reseña es obligatorio.")]
    public string Comentario { get; set; } = null!;

    [Column("fecha_resena")]
    public DateTime FechaResena { get; set; } = DateTime.Now;

    [Column("estado")]
    public bool Estado { get; set; } = true;

    [ForeignKey("UsuarioId")]
    public virtual Usuario? Usuario { get; set; }
}
