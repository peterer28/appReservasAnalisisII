using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Credencial
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(255)]
        public string Hash { get; set; }
    }
}
