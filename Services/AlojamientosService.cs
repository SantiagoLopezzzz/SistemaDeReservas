using sistemaReservas.Repositories;
using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public class AlojamientosService: IAlojamientosService
    {
        private readonly IAlojamientosRepository _repository;

        public AlojamientosService(IAlojamientosRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Alojamientos>> ObtenerTodas()
        {
            return await _repository.ObtenerTodas();
        }
        public async Task<Alojamientos?> ObtenerPorId(int? id)
        {
            return await _repository.ObtenerPorId(id);
        }

        public async Task Crear(Alojamientos Alojamiento)
        {
            await _repository.Crear(Alojamiento);
        }

        public async Task Actualizar(Alojamientos Alojamiento)
        {
            await _repository.Actualizar(Alojamiento);
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
