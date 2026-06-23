using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public class SedesRepository : ISedesRepository
    {
        private readonly ApplicationDbContext _context;

        public SedesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sedes>> ObtenerTodas()
        {
            return await _context.Sedes.ToListAsync();
        }
        public async Task<Sedes?> ObtenerPorId(int? id)
        {
            return await _context.Sedes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Crear(Sedes sede)
        {
            _context.Add(sede);

            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Sedes sede)
        {
            _context.Update(sede);

            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int? id)
        {
            var sede = await _context.Sedes.FindAsync(id);

            if (sede != null)
            {
                _context.Sedes.Remove(sede);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> Existe(int? id)
        {
            return await _context.Sedes.AnyAsync(x => x.Id == id);
        }

    }
}
