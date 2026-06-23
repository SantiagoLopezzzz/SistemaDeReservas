namespace SistemaReservas.Models
{
    public class Tarifa
    {
        public int Id { get; set; }

        public int AlojamientoId { get; set; }

        public int TemporadaId { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal PrecioPersonaAdicional { get; set; }

        public Alojamientos Alojamiento { get; set; }

    }
}