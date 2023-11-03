using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }
    }
}
