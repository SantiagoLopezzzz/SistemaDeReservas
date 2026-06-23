using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sistemaReservas.Services;
using SistemaReservas.Models;
using System.Security.Claims;

namespace sistemaReservas.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsuariosService _service;
        private readonly IEmailService _emailService;

        public AuthController(IUsuariosService service, IEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }

        // ── LOGIN ──────────────────────────────────────────────────────────

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Documento, string password)
        {
            var usuarios = await _service.ObtenerTodas();
            var usuario = usuarios.FirstOrDefault(x => x.Documento == Documento);

            if (usuario == null)
            {
                ViewBag.Error = "Documento o contraseña incorrectos.";
                return View();
            }

            var hasher = new PasswordHasher<Usuarios>();
            var resultado = hasher.VerifyHashedPassword(usuario, usuario.Passwordd, password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Documento o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Sedes");
        }

        // ── LOGOUT ────────────────────────────────────────────────────────

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        // ── RECUPERACION DE CONTRASEÑA ────────────────────────────────────

        // GET: Muestra el formulario (pide documento + respuesta secreta)
        public IActionResult RecuperarClave()
        {
            return View();
        }

        // POST: Valida la respuesta secreta, genera clave temporal y envía correo
        [HttpPost]
        public async Task<IActionResult> RecuperarClave(string documento, string respuestaSecreta)
        {
            var usuarios = await _service.ObtenerTodas();
            var usuario = usuarios.FirstOrDefault(x => x.Documento == documento);

            // Mensaje genérico para no revelar si el documento existe
            const string msgError = "Los datos ingresados no coinciden con ninguna cuenta.";

            if (usuario == null)
            {
                ViewBag.Error = msgError;
                return View();
            }

            // Comparar respuesta secreta (ignorar mayúsculas/espacios extra)
            bool respuestaOk = string.Equals(
                usuario.RespuestaSecreta.Trim(),
                respuestaSecreta.Trim(),
                StringComparison.OrdinalIgnoreCase);

            if (!respuestaOk)
            {
                ViewBag.Error = msgError;
                return View();
            }

            // Generar contraseña temporal aleatoria de 8 caracteres
            string nuevaClave = GenerarClaveAleatoria(8);

            // Hashear y guardar la nueva clave
            var hasher = new PasswordHasher<Usuarios>();
            usuario.Passwordd = hasher.HashPassword(usuario, nuevaClave);
            await _service.Actualizar(usuario);

            // Enviar correo con la clave temporal
            try
            {
                await _emailService.EnviarRecordacionContrasenaAsync(
                    usuario.Correo,
                    usuario.NombreCompleto,
                    nuevaClave);

                ViewBag.Exito = "Se ha enviado una contraseña temporal a tu correo registrado.";
            }
            catch
            {
                // Si falla el correo, igual informamos (en producción loguear el error)
                ViewBag.Exito = "Se genero una nueva contraseña temporal. " +
                                "Si no recibes el correo, contacta al administrador.";
            }

            return View();
        }

        // ── HELPERS ───────────────────────────────────────────────────────

        private static string GenerarClaveAleatoria(int longitud)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
            var random = new Random();
            return new string(Enumerable.Range(0, longitud)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}
