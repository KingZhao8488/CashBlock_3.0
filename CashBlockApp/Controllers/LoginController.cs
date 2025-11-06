using CashBlockApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashBlockApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly CashblockContext _context;

        public LoginController(CashblockContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string nombreUsuario, string contraseña)
        {
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Ingrese usuario y contraseña";
                return View();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdPerfilesNavigation)
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña);

            if (usuario != null)
            {
                // Verificar que tenga un perfil asignado
                if (usuario.IdPerfiles == null)
                {
                    ViewBag.Error = "Tu usuario no tiene un perfil asignado. Contacta al administrador.";
                    return View();
                }

                // Guardar datos en sesión
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
                HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
                HttpContext.Session.SetInt32("IdPerfiles", usuario.IdPerfiles.Value);

                // Redirigir según el rol
                switch (usuario.IdPerfiles.Value)
                {
                    case 1: // Administrador
                        return RedirectToAction("Index", "Home");
                    case 2: // Vendedor
                        return RedirectToAction("Index", "Vendedor");
                    case 3: // Bodeguero
                        return RedirectToAction("Inventario", "Bodeguero");
                    case 4: // Cajero
                        return RedirectToAction("Index", "Home");
                    case 5: // Cliente
                        return RedirectToAction("Index", "Cliente");
                    default:
                        ViewBag.Error = "Tu perfil no tiene permisos configurados. Contacta al administrador.";
                        return View();
                }
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }
        }

        // Cerrar sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}