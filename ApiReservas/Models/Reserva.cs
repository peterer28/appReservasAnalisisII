using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }
        public int IdHabitacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Pago { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FechaIngreso { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FechaSalida { get; set; }

        [ForeignKey("IdUsuario")]
        [Required]
        public Usuario Usuario { get; set; }

        [ForeignKey("IdHabitacion")]
        [Required]
        public Habitacion Habitacion { get; set; }
    }
}
