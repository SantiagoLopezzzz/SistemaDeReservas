using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuariosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuarios>> ObtenerTodas()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuarios?> ObtenerPorId(int? id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Crear(Usuarios usuario)
        {
            _context.Add(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Usuarios usuario)
        {
            _context.Update(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int? id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> Existe(int? id)
        {
            return await _context.Sedes.AnyAsync(x => x.Id == id);
        }

    }
}
