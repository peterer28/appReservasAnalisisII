using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Permiso
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoUsuario { get; set; }
    }
}
