using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public class ReservasRepository : IReservasRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservas>> ObtenerTodas()
        {
            return await _context.Reservas
                .Include(x => x.Alojamiento)
                .ToListAsync();
        }
        public async Task<Reservas?> ObtenerPorId(int? id)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Crear(Reservas Reserva)
        {
            _context.Add(Reserva);

            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Reservas Reserva)
        {
            _context.Update(Reserva);

            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int? id)
        {
            var reserva = await _context.Alojamientos.FindAsync(id);

            if (reserva != null)
            {
                _context.Alojamientos.Remove(reserva);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> Existe(int? id)
        {
            return await _context.Alojamientos.AnyAsync(x => x.Id == id);
        }

    }
}
