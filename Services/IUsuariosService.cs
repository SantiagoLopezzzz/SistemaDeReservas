using SistemaReservas.Models;

namespace sistemaReservas.Services
{
    public interface IUsuariosService
    {
        Task<IEnumerable<Usuarios>> ObtenerTodas();

        Task<Usuarios?> ObtenerPorId(int? id);

        Task Crear(Usuarios usuario);

        Task Actualizar(Usuarios usuario);

        Task Eliminar(int? id);

        Task<bool> Existe(int? id);




    }
}
