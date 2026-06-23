using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public interface ISedesRepository
    {
        Task<IEnumerable<Sedes>> ObtenerTodas();

        Task<Sedes?> ObtenerPorId(int? id);

        Task Crear(Sedes sede);

        Task Actualizar(Sedes sede);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);
    }
}
