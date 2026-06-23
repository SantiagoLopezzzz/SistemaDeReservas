using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public interface IReservasService
    {
        Task<IEnumerable<Reservas>> ObtenerTodas();

        Task<Reservas> ObtenerPorId(int? id);

        Task Crear(Reservas Reserva);

        Task Actualizar(Reservas Reserva);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);




    }
}
