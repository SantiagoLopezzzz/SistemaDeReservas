using Microsoft.AspNetCore.Mvc;
using sistemaReservas.Models;
using SistemaReservas.Data;
using System.Diagnostics;

namespace sistemaReservas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TotalSedes =
        _context.Sedes.Count();

            ViewBag.TotalAlojamientos =
                _context.Alojamientos.Count();

            ViewBag.TotalReservas =
                _context.Reservas.Count();

            ViewBag.ReservasActivas =
                _context.Reservas.Count(r => r.Estado);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
