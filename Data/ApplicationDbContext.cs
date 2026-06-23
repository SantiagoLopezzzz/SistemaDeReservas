using Microsoft.EntityFrameworkCore;
using sistemaReservas.Models;
using SistemaReservas.Models;

namespace SistemaReservas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sedes> Sedes { get; set; }

        public DbSet<Alojamientos> Alojamientos { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<Reservas> Reservas { get; set; }
    }
}
