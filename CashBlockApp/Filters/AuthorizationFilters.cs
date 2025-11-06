using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashBlockApp.Filters
{
    public class AuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // Permitir acceso al Login sin restricciones
            if (controller == "Login")
            {
                return;
            }

            // Verificar si el usuario está logueado
            var usuarioId = context.HttpContext.Session.GetInt32("IdUsuario");
            var perfilId = context.HttpContext.Session.GetInt32("IdPerfiles");

            if (usuarioId == null || perfilId == null)
            {
                // Redirigir al login si no está autenticado
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Verificar permisos por rol
            if (!TienePermiso(perfilId.Value, controller))
            {
                // Redirigir a página de acceso denegado
                context.Result = new RedirectToActionResult("AccesoDenegado", "Home", null);
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se necesita implementación
        }

        private bool TienePermiso(int perfilId, string controller)
        {
            // Definir permisos por rol
            switch (perfilId)
            {
                case 1: // Administrador - Acceso total
                    return true;

                case 2: // Vendedor
                    var controladoresVendedor = new[] { "Vendedor", "Home" };
                    return controladoresVendedor.Contains(controller);

                case 3: // Bodeguero
                    var controladoresBodeguero = new[] { "Bodeguero", "Existencias", "Productos", "Home" };
                    return controladoresBodeguero.Contains(controller);

                case 4: // Cajero
                    var controladoresCajero = new[] { "Ventas", "Facturas", "MetodoPagos", "Home" };
                    return controladoresCajero.Contains(controller);

                case 5: // Cliente
                    var controladoresCliente = new[] { "Cliente", "Home" };
                    return controladoresCliente.Contains(controller);

                default:
                    return false;
            }
        }
    }
}