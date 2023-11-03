using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class TipoHabitacion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Tipo { get; set; }
    }
}
