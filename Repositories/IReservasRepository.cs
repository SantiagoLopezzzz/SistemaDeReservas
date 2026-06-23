using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public interface IReservasRepository
    {
        Task<IEnumerable<Reservas>> ObtenerTodas();

        Task<Reservas?> ObtenerPorId(int? id);

        Task Crear(Reservas Alojamiento);

        Task Actualizar(Reservas Alojamiento);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);
    }
}
