namespace SistemaReservas.Models
{
    public class Reservas
    {
        public int Id { get; set; }

        public DateTime FechaReserva { get; set; }

        public DateTime FechaLLegada { get; set; }

        public DateTime FechaSalida { get; set; }


        public int cantidadPersonas { get; set; }

        public int TipoTarifa { get; set; }

        public decimal valorTotal { get; set; }

        public bool Estado { get; set; }

        //Foreign Key para Alojamientos
        public int AlojamientoId { get; set; }

        public Alojamientos? Alojamiento { get; set; }

        //Foreign Key para Usuarios
        public int UsuarioId { get; set; }

        public Usuarios? Usuario { get; set; }
    }
}