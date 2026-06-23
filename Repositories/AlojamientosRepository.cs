using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public class AlojamientosRepository : IAlojamientosRepository
    {
        private readonly ApplicationDbContext _context;

        public AlojamientosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Alojamientos>> ObtenerTodas()
        {
            return await _context.Alojamientos
                .Include(x => x.Sede)
                .ToListAsync();
        }
        public async Task<Alojamientos?> ObtenerPorId(int? id)
        {
            return await _context.Alojamientos
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Crear(Alojamientos Alojamiento)
        {
            _context.Add(Alojamiento);

            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Alojamientos Alojamiento)
        {
            _context.Update(Alojamiento);

            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int? id)
        {
            var alojamiento = await _context.Alojamientos.FindAsync(id);

            if (alojamiento != null)
            {
                _context.Alojamientos.Remove(alojamiento);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> Existe(int? id)
        {
            return await _context.Alojamientos.AnyAsync(x => x.Id == id);
        }

    }
}
