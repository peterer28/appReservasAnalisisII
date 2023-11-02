using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiReservas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string AMaterno { get; set; }

        [Required]
        [StringLength(30)]
        public string APaterno { get; set; }

        public int IdContacto { get; set; }
        public int IdCredenciales { get; set; }
        public int IdPermisos { get; set; }

        [ForeignKey("IdContacto")]
        [Required]
        public Contacto Contacto { get; set; }

        [ForeignKey("IdCredenciales")]
        [Required]
        public Credencial Credenciales { get; set; }

        [ForeignKey("IdPermisos")]
        [Required]
        public Permiso Permisos { get; set; }
    }
}
