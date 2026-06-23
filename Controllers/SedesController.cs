
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaReservas.Services;
using SistemaReservas.Data;
using SistemaReservas.Models;

public class SedesController : Controller
{
    private readonly ISedesService _service;
    public SedesController(ISedesService service)
    {
        _service = service;
    }

    // GET: SEDESS
    public async Task<IActionResult> Index()    
    {
        var sedes = await _service.ObtenerTodas();
        return View(sedes);
        
    }

    // GET: SEDESS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var sedes = await _service.ObtenerPorId(id);
        if (sedes == null)
        {
            return NotFound();
        }

        return View(sedes);
    }

    // GET: SEDESS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SEDESS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Ubicacion,Descripcion,Tipo")] Sedes sedes)
    {
        if (ModelState.IsValid)
        {
            await _service.Crear(sedes);
            return RedirectToAction(nameof(Index));
        }
        return View(sedes);
    }

    // GET: SEDESS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sedes = await _service.ObtenerPorId(id);
        if (sedes == null)
        {
            return NotFound();
        }
        return View(sedes);
    }

    // POST: SEDESS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Ubicacion,Descripcion,Tipo")] Sedes sedes)
    {
        if (id != sedes.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _service.Actualizar(sedes);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _service.Existe(sedes.Id))
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
        return View(sedes);
    }

    // GET: SEDESS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sedes = await _service.ObtenerPorId(id);
        if (sedes == null)
        {
            return NotFound();
        }

        return View(sedes);
    }

    // POST: SEDESS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        await _service.Eliminar(id);

        return RedirectToAction(nameof(Index));
    }

    
}
