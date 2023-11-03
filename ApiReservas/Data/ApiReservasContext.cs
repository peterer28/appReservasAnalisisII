using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiReservas.Models;

namespace ApiReservas.Data
{
    public class ApiReservasContext : DbContext
    {
        public ApiReservasContext (DbContextOptions<ApiReservasContext> options)
            : base(options)
        {
        }

        public DbSet<ApiReservas.Models.Credencial> Credencial { get; set; } = default!;

        public DbSet<ApiReservas.Models.Contacto> Contacto { get; set; } = default!;

        public DbSet<ApiReservas.Models.Permiso> Permiso { get; set; } = default!;

        public DbSet<ApiReservas.Models.Usuario> Usuario { get; set; } = default!;

        public DbSet<ApiReservas.Models.TipoHabitacion> TipoHabitacion { get; set; } = default!;

        public DbSet<ApiReservas.Models.Cama> Cama { get; set; } = default!;

        public DbSet<ApiReservas.Models.Hotel> Hotel { get; set; } = default!;

        public DbSet<ApiReservas.Models.Habitacion> Habitacion { get; set; } = default!;

        public DbSet<ApiReservas.Models.Reserva> Reserva { get; set; } = default!;

        public DbSet<ApiReservas.Models.MatrizBloqueo> MatrizBloqueo { get; set; } = default!;

        public DbSet<ApiReservas.Models.MatrizBloqueoDetalle> MatrizBloqueoDetalle { get; set; } = default!;
    }
}
