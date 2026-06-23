namespace SistemaReservas.Models
{
    public class Sedes
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Ubicacion { get; set; }

        public string Descripcion { get; set; }

        public int Tipo { get; set; }

        // Relación con Alojamientos
        public ICollection<Alojamientos> Alojamientos { get; set; }
            = new List<Alojamientos>();
    }
}