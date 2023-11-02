using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Cama
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Disposicion { get; set; }
    }
}
