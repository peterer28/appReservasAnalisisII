using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiReservas.Models
{
    public class MatrizBloqueo
    {
        public int Id { get; set; }
        public int IdHabitacion { get; set; }
        public int ReservationYear { get; set; }

        [ForeignKey("IdHabitacion")]
        [Required]
        public Habitacion Habitacion { get; set; }
    }
}
