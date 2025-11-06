using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashBlockApp.Controllers
{
    public class HomeController : Controller
    {
        // Esto se ejecuta antes de cualquier acción en este controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            if (usuarioId == null)
            {
                // Si no hay usuario logueado, redirige al login
                context.Result = RedirectToAction("Index", "Login");
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            var perfilId = HttpContext.Session.GetInt32("IdPerfiles");
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");

            ViewBag.PerfilId = perfilId;
            ViewBag.NombreUsuario = nombreUsuario;

            // Redirigir según el rol
            switch (perfilId)
            {
                case 1: // Administrador
                    ViewBag.Rol = "Administrador";
                    break;
                case 2: // Vendedor
                    return RedirectToAction("Index", "Vendedor");
                case 3: // Bodeguero
                    return RedirectToAction("Inventario", "Bodeguero");
                case 4: // Cajero
                    ViewBag.Rol = "Cajero";
                    break;
                case 5: // Cliente
                    return RedirectToAction("Index", "Cliente");
                default:
                    ViewBag.Rol = "Usuario";
                    break;
            }

            return View();
        }

        // Vista de acceso denegado
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}