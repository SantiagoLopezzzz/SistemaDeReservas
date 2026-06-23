namespace SistemaReservas.Models
{
    public class Alojamientos
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int Capacidad { get; set; }

        public decimal TarifaDiaOrdinario { get; set; }

        public decimal TarifaDiaEspecial { get; set; }

        public bool Estado { get; set; }

        //Foreign Key para Sedes
        public int SedeId { get; set; }

        public Sedes? Sede { get; set; }
    }
}