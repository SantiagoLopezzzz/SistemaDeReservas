namespace sistemaReservas.Services
{
    public interface IEmailService
    {
        Task EnviarRecordacionContrasenaAsync(string destinatario, string nombreUsuario, string nuevaClave);
        Task EnviarConfirmacionReservaAsync(string destinatario, string nombreUsuario, int reservaId);
    }
}
