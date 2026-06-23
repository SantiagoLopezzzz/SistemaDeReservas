using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public interface IAlojamientosService
    {
        Task<IEnumerable<Alojamientos>> ObtenerTodas();

        Task<Alojamientos?> ObtenerPorId(int? id);

        Task Crear(Alojamientos Alojamiento);

        Task Actualizar(Alojamientos Alojamiento);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);




    }
}
