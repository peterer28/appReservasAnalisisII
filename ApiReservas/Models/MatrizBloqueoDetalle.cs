using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiReservas.Models
{
    public class MatrizBloqueoDetalle
    {
        public int Id { get; set; }
        public int IdMatrizBloqueo { get; set; }
        public int DayOfYear { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey("IdMatrizBloqueo")]
        [Required]
        public MatrizBloqueo MatrizBloqueo { get; set; }
    }
}
