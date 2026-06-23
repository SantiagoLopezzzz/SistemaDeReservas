using sistemaReservas.Repositories;
using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public class SedesService: ISedesService
    {
        private readonly ISedesRepository _repository;

        public SedesService(ISedesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sedes>> ObtenerTodas()
        {
            return await _repository.ObtenerTodas();
        }
        public async Task<Sedes?> ObtenerPorId(int? id)
        {
            return await _repository.ObtenerPorId(id);
        }

        public async Task Crear(Sedes sede)
        {
            await _repository.Crear(sede);
        }

        public async Task Actualizar(Sedes sede)
        {
            await _repository.Actualizar(sede);
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
