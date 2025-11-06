using CashBlockApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CashBlockApp.Controllers
{
    public class ClientesAdminController : Controller
    {
        private readonly CashblockContext _context;

        public ClientesAdminController(CashblockContext context)
        {
            _context = context;
        }

        // Verificar que sea administrador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var perfilId = HttpContext.Session.GetInt32("IdPerfiles");

            if (usuarioId == null)
            {
                context.Result = RedirectToAction("Index", "Login");
            }
            else if (perfilId != 1) // Solo administradores
            {
                context.Result = RedirectToAction("AccesoDenegado", "Home");
            }

            base.OnActionExecuting(context);
        }

        // GET: ClientesAdmin
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdPerfilesNavigation)
                .ToListAsync();

            return View(clientes);
        }

        // GET: ClientesAdmin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdPerfilesNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.CedulaCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: ClientesAdmin/Create
        public IActionResult Create()
        {
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "Rol");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario");
            return View();
        }

        // POST: ClientesAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CedulaCliente,Nombre,Apellido,Telefono,Direccion,Email,IdUsuario,IdPerfiles")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "Rol", cliente.IdPerfiles);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", cliente.IdUsuario);
            return View(cliente);
        }

        // GET: ClientesAdmin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "Rol", cliente.IdPerfiles);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", cliente.IdUsuario);
            return View(cliente);
        }

        // POST: ClientesAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CedulaCliente,Nombre,Apellido,Telefono,Direccion,Email,IdUsuario,IdPerfiles")] Cliente cliente)
        {
            if (id != cliente.CedulaCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cliente actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.CedulaCliente))
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
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "Rol", cliente.IdPerfiles);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", cliente.IdUsuario);
            return View(cliente);
        }

        // GET: ClientesAdmin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdPerfilesNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.CedulaCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: ClientesAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cliente eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(string id)
        {
            return _context.Clientes.Any(e => e.CedulaCliente == id);
        }
    }
}