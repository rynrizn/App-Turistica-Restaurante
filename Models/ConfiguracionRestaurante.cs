using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteTuristicoApp.Models
{
    [Table("configuracion_restaurante")]
    public class ConfiguracionRestaurante
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("clave")]
        public string Clave { get; set; } = string.Empty;

        [Required]
        [Column("valor")]
        public string Valor { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}
