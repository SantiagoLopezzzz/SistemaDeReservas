
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

[Authorize(Roles = "Admin, Usuario")]
public class ReservasController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: RESERVASS
    public async Task<IActionResult> Index()    
    {
        if (User.IsInRole("Admin"))
        {
            var todasLasReservas = await _context.Reservas
                .Include(r => r.Alojamiento)
                .ThenInclude(a => a.Sede)
                .ToListAsync();

            return View(todasLasReservas);
        }
        var usuarioId = int.Parse(
            User.Claims.First(x => x.Type == "UsuarioId").Value);

        var reservas = await _context.Reservas
            .Where(r => r.UsuarioId == usuarioId)
            .Include(r => r.Alojamiento)
            .ThenInclude(a => a.Sede)
            .ToListAsync();

        return View(reservas);
    }

    // GET: RESERVASS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservas = await _context.Reservas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reservas == null)
        {
            return NotFound();
        }

        return View(reservas);
    }

    // GET: RESERVASS/Create
    public IActionResult Create(int id)
    {
        ViewBag.NombreAlojamiento = _context.Alojamientos
        .FirstOrDefault(a => a.Id == id)?.Nombre;

        Reservas reserva = new Reservas
        {
            AlojamientoId = id,
            FechaLLegada = DateTime.Today,
            FechaSalida = DateTime.Today.AddDays(1)
        };

        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FechaLLegada,FechaSalida,cantidadPersonas,Estado,AlojamientoId,TipoTarifa")] Reservas reservas)
    {
        reservas.FechaReserva = DateTime.Now;

        bool ocupado = await _context.Reservas.AnyAsync(r =>
            r.AlojamientoId == reservas.AlojamientoId &&
            reservas.FechaLLegada < r.FechaSalida &&
            reservas.FechaSalida > r.FechaLLegada
        );

        if (reservas.FechaSalida <= reservas.FechaLLegada)
        {
            ModelState.AddModelError("",
                "La fecha de salida debe ser mayor a la de llegada.");

            ViewBag.Alojamiento = new SelectList(
                _context.Alojamientos,
                "Id",
                "Nombre",
                reservas.AlojamientoId
            );

            return View(reservas);
        }


        if (ocupado)
        {
            ModelState.AddModelError("",
                "El alojamiento ya está reservado en esas fechas.");

            ViewBag.Alojamiento = new SelectList(
                _context.Alojamientos,
                "Id",
                "Nombre",
                reservas.AlojamientoId
            );

            return View(reservas);
        }
        // Validar capacidad del alojamiento
        var alojamiento = await _context.Alojamientos.FirstOrDefaultAsync(a => a.Id == reservas.AlojamientoId);


        if (alojamiento != null)
        {
            if (reservas.cantidadPersonas > alojamiento.Capacidad)
            {
                ModelState.AddModelError("",
                    "La cantidad de personas supera la capacidad.");
            }
        }
        // Calcular valor total
        if (alojamiento != null)
        {
            int Noches = (reservas.FechaSalida - reservas.FechaLLegada).Days;

            if (reservas.TipoTarifa == 2)
            {
                reservas.valorTotal =
                    Noches * alojamiento.TarifaDiaEspecial;
            }
            else
            {
                reservas.valorTotal =
                    Noches * alojamiento.TarifaDiaOrdinario;
            }
        }

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
        if (ModelState.IsValid)
        {
            var usuarioId = User.Claims
                .FirstOrDefault(x => x.Type == "UsuarioId")?.Value;

            reservas.UsuarioId = int.Parse(usuarioId);
            _context.Add(reservas);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Alojamiento = new SelectList(
        _context.Alojamientos,
        "Id",
        "Nombre",
        reservas.AlojamientoId
    );
        return View(reservas);
    }

    // GET: RESERVASS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservas = await _context.Reservas.FindAsync(id);
        if (reservas == null)
        {
            return NotFound();
        }
        ViewBag.Alojamiento = new SelectList(
        _context.Alojamientos,
        "Id",
        "Nombre",
        reservas.AlojamientoId);
        return View(reservas);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    int? id,
    [Bind("Id,FechaReserva,FechaLLegada,FechaSalida,cantidadPersonas,Estado,AlojamientoId,TipoTarifa")]
    Reservas reservas)
    {
        if (id != reservas.Id)
        {
            return NotFound();
        }

        if (reservas.FechaSalida <= reservas.FechaLLegada)
        {
            ModelState.AddModelError("",
                "La fecha de salida debe ser mayor a la de llegada.");

            return View(reservas);
        }

        bool ocupado = await _context.Reservas.AnyAsync(r =>

            r.AlojamientoId == reservas.AlojamientoId &&

            r.Id != reservas.Id &&

            reservas.FechaLLegada < r.FechaSalida &&

            reservas.FechaSalida > r.FechaLLegada
        );

        if (ocupado)
        {
            ModelState.AddModelError("",
                "El alojamiento ya está reservado en esas fechas.");

            return View(reservas);
        }

        // BUSCAR RESERVA ORIGINAL
        var reservaBD = await _context.Reservas
            .FirstOrDefaultAsync(r => r.Id == reservas.Id);

        if (reservaBD == null)
        {
            return NotFound();
        }

        // OBTENER ALOJAMIENTO
        var alojamiento = await _context.Alojamientos
            .FirstOrDefaultAsync(a => a.Id == reservas.AlojamientoId);

        // ACTUALIZAR CAMPOS
        reservaBD.FechaLLegada = reservas.FechaLLegada;
        reservaBD.FechaSalida = reservas.FechaSalida;
        reservaBD.cantidadPersonas = reservas.cantidadPersonas;
        reservaBD.Estado = reservas.Estado;
        reservaBD.AlojamientoId = reservas.AlojamientoId;
        reservaBD.TipoTarifa = reservas.TipoTarifa;

        // CALCULAR VALOR
        if (alojamiento != null)
        {
            int Noches =
                (reservas.FechaSalida - reservas.FechaLLegada).Days;

            if (reservas.TipoTarifa == 2)
            {
                reservaBD.valorTotal =
                    Noches * alojamiento.TarifaDiaEspecial;
            }
            else
            {
                reservaBD.valorTotal =
                    Noches * alojamiento.TarifaDiaOrdinario;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: RESERVASS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservas = await _context.Reservas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reservas == null)
        {
            return NotFound();
        }

        return View(reservas);
    }

    // POST: RESERVASS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var reservas = await _context.Reservas.FindAsync(id);
        if (reservas != null)
        {
            _context.Reservas.Remove(reservas);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReservasExists(int? id)
    {
        return _context.Reservas.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Comprobante(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reserva = await _context.Reservas
            .Include(r => r.Alojamiento)
            .ThenInclude(a => a.Sede)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }
}
