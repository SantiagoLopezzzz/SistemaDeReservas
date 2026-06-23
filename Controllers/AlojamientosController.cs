
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaReservas.Data;
using SistemaReservas.Models;

public class AlojamientosController : Controller
{
    private readonly ApplicationDbContext _context;

    public AlojamientosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ALOJAMIENTOSS
    public async Task<IActionResult> Index(int? sedeId)    
    {
        var alojamientos = _context.Alojamientos
        .Include(a => a.Sede)
        .AsQueryable();

        if (sedeId != null)
        {
            alojamientos = alojamientos
                .Where(a => a.SedeId == sedeId.Value);
        }
        return View(await alojamientos.ToListAsync());
    }

    // GET: ALOJAMIENTOSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alojamientos = await _context.Alojamientos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (alojamientos == null)
        {
            return NotFound();
        }

        return View(alojamientos);
    }

    // GET: ALOJAMIENTOSS/Create
    public IActionResult Create()
    {
        ViewBag.Sede = new SelectList(
        _context.Sedes,
        "Id",
        "Nombre"
    );
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Capacidad,TarifaDiaOrdinario,TarifaDiaEspecial,Estado,SedeId")] Alojamientos alojamientos)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

            if (ModelState.IsValid)
        {
            _context.Add(alojamientos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Sede = new SelectList(
        _context.Sedes,
        "Id",
        "Nombre",
        alojamientos.SedeId
    );
        return View(alojamientos);
    }

    // GET: ALOJAMIENTOSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alojamientos = await _context.Alojamientos.FindAsync(id);

        if (alojamientos == null)
        {
            return NotFound();

        }

        ViewBag.Sede = new SelectList(
        _context.Sedes,
        "Id",
        "Nombre",
        alojamientos.SedeId);

        return View(alojamientos);
    }

    // POST: ALOJAMIENTOSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Capacidad,TarifaDiaOrdinario,TarifaDiaEspecial,Estado,SedeId")] Alojamientos alojamientos)
    {
        if (id != alojamientos.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(alojamientos);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlojamientosExists(alojamientos.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(alojamientos);
    }

    // GET: ALOJAMIENTOSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alojamientos = await _context.Alojamientos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (alojamientos == null)
        {
            return NotFound();
        }

        return View(alojamientos);
    }

    // POST: ALOJAMIENTOSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var alojamientos = await _context.Alojamientos.FindAsync(id);
        if (alojamientos != null)
        {
            _context.Alojamientos.Remove(alojamientos);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AlojamientosExists(int? id)
    {
        return _context.Alojamientos.Any(e => e.Id == id);
    }
    public async Task<IActionResult> Calendario(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservas = await _context.Reservas
            .Where(r => r.AlojamientoId == id)
            .ToListAsync();

        ViewBag.Alojamiento = await _context.Alojamientos
            .FirstOrDefaultAsync(a => a.Id == id);

        return View(reservas);
    }


    //SP
    public async Task<IActionResult> Disponibles(
        DateTime fechaLlegada,
        DateTime fechaSalida)
    {
        var disponibles = await _context.Alojamientos
            .FromSqlRaw(
                "EXEC sp_HabitacionesDisponibles @p0, @p1",
                fechaLlegada,
                fechaSalida)
            .ToListAsync();

        return View(disponibles);
    }

}


