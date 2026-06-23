using System.ComponentModel.DataAnnotations;

namespace SistemaReservas.Models
{
    public class Usuarios
    {
        public int Id { get; set; }

        [Required]
        public string Documento { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Celular { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Departamento { get; set; }

        [Required]
        public string Municipio { get; set; }

        [Required]
        public string Barrio { get; set; }

        [Required]
        public string DireccionResidencia { get; set; }

        public string? TelefonoResidencia { get; set; }

        [Required]
        public string PreguntaSecreta { get; set; }

        [Required]
        public string RespuestaSecreta { get; set; }

        public bool AutorizaCorreo { get; set; }

        public bool AutorizaCelular { get; set; }

        [Required]
        public string Passwordd { get; set; }

        public string Rol { get; set; } = "Usuario";

        public ICollection<Reservas>? Reservas { get; set; }
    }
}