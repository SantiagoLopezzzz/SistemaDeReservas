using sistemaReservas.Repositories;
using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public class ReservasService: IReservasService
    {
        private readonly IReservasRepository _repository;

        public ReservasService(IReservasRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Reservas>> ObtenerTodas()
        {
            return await _repository.ObtenerTodas();
        }
        public async Task<Reservas?> ObtenerPorId(int? id)
        {
            return await _repository.ObtenerPorId(id);
        }

        public async Task Crear(Reservas Reserva)
        {
            await _repository.Crear(Reserva);
        }

        public async Task Actualizar(Reservas Reserva)
        {
            await _repository.Actualizar(Reserva);
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
