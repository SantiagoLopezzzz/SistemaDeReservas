using sistemaReservas.Repositories;
using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public class UsuariosService: IUsuariosService
    {
        private readonly IUsuariosRepository _repository;

        public UsuariosService(IUsuariosRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Usuarios>> ObtenerTodas()
        {
            return await _repository.ObtenerTodas();
        }
        public async Task<Usuarios?> ObtenerPorId(int? id)
        {
            return await _repository.ObtenerPorId(id);
        }

        public async Task Crear(Usuarios usuario)
        {
            await _repository.Crear(usuario);
        }

        public async Task Actualizar(Usuarios usuario)
        {
            await _repository.Actualizar(usuario);
        }

        public async Task Eliminar(int? id)
        {
            await _repository.Eliminar(id);
        }

        public async Task<bool> Existe(int? id)
        {
            return await _repository.Existe(id);
        }
    }
}
