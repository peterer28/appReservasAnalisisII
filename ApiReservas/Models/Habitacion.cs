using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiReservas.Models
{
    public class Habitacion
    {
        public int Id { get; set; }

        public int IdHotel { get; set; }
        public int IdTipoHabitacion { get; set; }
        public int IdCamas { get; set; }

        [ForeignKey("IdHotel")]
        [Required]
        public Hotel Hotel { get; set; }

        [ForeignKey("IdTipoHabitacion")]
        [Required]
        public TipoHabitacion TipoHabitacion { get; set; }

        [ForeignKey("IdCamas")]
        [Required]
        public Cama Camas { get; set; }
    }
}
