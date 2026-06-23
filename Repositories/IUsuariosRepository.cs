using SistemaReservas.Models;

namespace sistemaReservas.Repositories
{
    public interface IUsuariosRepository
    {
        Task<IEnumerable<Usuarios>> ObtenerTodas();

        Task<Usuarios?> ObtenerPorId(int? id);

        Task Crear(Usuarios usuario);

        Task Actualizar(Usuarios usuario);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);
    }
}
