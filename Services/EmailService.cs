using System.Net;
using System.Net.Mail;

namespace sistemaReservas.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // Metodo base para enviar cualquier correo
        private async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"]!);
            var smtpUser = _config["Smtp:Usuario"];
            var smtpPassword = _config["Smtp:Password"];
            var remitente = _config["Smtp:Remitente"];
            var nombreRemit = _config["Smtp:NombreRemitente"] ?? "Sistema de Reservas FODUN";

            using var cliente = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            var mensaje = new MailMessage
            {
                From = new MailAddress(remitente!, nombreRemit),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            mensaje.To.Add(destinatario);

            await cliente.SendMailAsync(mensaje);
        }

        // Recordacion / recuperacion de contrasena
        public async Task EnviarRecordacionContrasenaAsync(
            string destinatario, string nombreUsuario, string nuevaClave)
        {
            string asunto = "Sistema de Reservas FODUN - Recuperacion de contrasena";

            string cuerpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:auto; border:1px solid #ddd; border-radius:8px; overflow:hidden;'>
                        <div style='background-color:#8B1A1A; padding:20px; text-align:center;'>
                            <h1 style='color:white; margin:0;'>FODUN</h1>
                            <p style='color:#f0d0d0; margin:5px 0 0;'>Sistema de Reservas</p>
                        </div>
                        <div style='padding:30px;'>
                            <h2>Hola, {nombreUsuario}</h2>
                            <p>Recibimos una solicitud de recuperacion de contrasena para tu cuenta.</p>
                            <p>Tu nueva contrasena temporal es:</p>
                            <div style='background:#f5f5f5; padding:15px; border-radius:6px;
                                        font-size:24px; font-weight:bold; text-align:center;
                                        letter-spacing:4px; color:#8B1A1A;'>
                                {nuevaClave}
                            </div>
                            <p style='margin-top:20px;'>
                                Por seguridad, te recomendamos cambiarla despues de iniciar sesion.
                            </p>
                            <p style='color:#999; font-size:12px; margin-top:30px;'>
                                Si no solicitaste este cambio, ignora este correo. Tu contrasena anterior
                                seguira activa.
                            </p>
                        </div>
                        <div style='background:#f9f0f0; padding:15px; text-align:center;
                                    font-size:12px; color:#999;'>
                            FODUN - Fondo de Docentes Universidad Nacional &copy; 2026
                        </div>
                    </div>
                </body>
                </html>";

            await EnviarAsync(destinatario, asunto, cuerpo);
        }

        // Confirmacion de reserva (bonus util para el sistema)
        public async Task EnviarConfirmacionReservaAsync(
            string destinatario, string nombreUsuario, int reservaId)
        {
            string asunto = $"FODUN - Confirmacion de reserva #{reservaId}";

            string cuerpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <div style='max-width:600px; margin:auto; border:1px solid #ddd; border-radius:8px; overflow:hidden;'>
                        <div style='background-color:#8B1A1A; padding:20px; text-align:center;'>
                            <h1 style='color:white; margin:0;'>FODUN</h1>
                            <p style='color:#f0d0d0; margin:5px 0 0;'>Sistema de Reservas</p>
                        </div>
                        <div style='padding:30px;'>
                            <h2>Hola, {nombreUsuario}</h2>
                            <p>Tu reserva ha sido confirmada exitosamente.</p>
                            <p><strong>Numero de reserva: #{reservaId}</strong></p>
                            <p>Puedes consultar todos los detalles de tu reserva iniciando sesion
                               en el sistema y visitando la seccion <em>Mis Reservas</em>.</p>
                        </div>
                        <div style='background:#f9f0f0; padding:15px; text-align:center;
                                    font-size:12px; color:#999;'>
                            FODUN - Fondo de Docentes Universidad Nacional &copy; 2026
                        </div>
                    </div>
                </body>
                </html>";

            await EnviarAsync(destinatario, asunto, cuerpo);
        }
    }
}