using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Direccion { get; set; }

        public int IdContacto { get; set; }

        [ForeignKey("IdContacto")]
        [Required]
        public Contacto Contacto { get; set; }
    }
}
