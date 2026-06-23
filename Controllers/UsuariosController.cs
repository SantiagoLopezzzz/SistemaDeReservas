using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaReservas.Services;
using SistemaReservas.Data;
using SistemaReservas.Models;



public class UsuariosController : Controller
{
    private readonly IUsuariosService _service;

    public UsuariosController(IUsuariosService service)
    {
        _service = service;
    }

    // GET: USUARIOSS
    public async Task<IActionResult> Index()    
    {
        return View(await _service.ObtenerTodas());
    }

    // GET: USUARIOSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuarios = await _service.ObtenerPorId(id);
        if (usuarios == null)
        {
            return NotFound();
        }

        return View(usuarios);
    }

    // GET: USUARIOSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: USUARIOSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Documento,NombreCompleto,FechaNacimiento,Celular,Correo,Departamento,Municipio,Barrio,DireccionResidencia,TelefonoResidencia,PreguntaSecreta,RespuestaSecreta,AutorizaCorreo,AutorizaCelular,Passwordd")] Usuarios usuarios)
    {
        usuarios.Rol = "Usuario";
        if (ModelState.IsValid)
        {
            var passwordHasher = new PasswordHasher<Usuarios>();

            usuarios.Passwordd = passwordHasher.HashPassword(usuarios, usuarios.Passwordd);

            await _service.Crear(usuarios);
            return RedirectToAction(nameof(Index));
        }
        return View(usuarios);
    }

    // GET: USUARIOSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuarios = await _service.ObtenerPorId(id);
        if (usuarios == null)
        {
            return NotFound();
        }
        return View(usuarios);
    }

    // POST: USUARIOSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Documento,NombreCompleto,FechaNacimiento,Celular,Correo,Departamento,Municipio,Barrio,DireccionResidencia,TelefonoResidencia,PreguntaSecreta,RespuestaSecreta,AutorizaCorreo,AutorizaCelular,Passwordd,Rol")] Usuarios usuarios)
    {
        if (id != usuarios.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _service.Actualizar(usuarios);
            return RedirectToAction(nameof(Index));
        }
        return View(usuarios);
    }

    // GET: USUARIOSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuarios = await _service.ObtenerPorId(id);
        if (usuarios == null)
        {
            return NotFound();
        }

        return View(usuarios);
    }

    // POST: USUARIOSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        await _service.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UsuariosExists(int? id)
    {
        return await _service.Existe(id);
    }
}
