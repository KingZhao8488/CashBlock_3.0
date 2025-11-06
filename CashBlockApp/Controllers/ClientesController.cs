using CashBlockApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CashBlockApp.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CashblockContext _context;

        public ClientesController(CashblockContext context)
        {
            _context = context;
        }

        // Verificar sesión y rol
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var perfilId = HttpContext.Session.GetInt32("IdPerfiles");

            if (usuarioId == null)
            {
                context.Result = RedirectToAction("Index", "Login");
            }
            else if (perfilId != 5) // Solo clientes
            {
                context.Result = RedirectToAction("Index", "Home");
            }

            base.OnActionExecuting(context);
        }

        // GET: Cliente/Index - Dashboard del cliente
        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            // Obtener datos del cliente
            var cliente = await _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (cliente == null)
            {
                ViewBag.Error = "No se encontró información del cliente";
                return View();
            }

            ViewBag.NombreCliente = $"{cliente.Nombre} {cliente.Apellido}";
            ViewBag.CedulaCliente = cliente.CedulaCliente;

            // Estadísticas del cliente
            var totalCompras = await _context.Venta
                .Where(v => v.CedulaCliente == cliente.CedulaCliente)
                .CountAsync();

            var totalGastado = await _context.Venta
                .Where(v => v.CedulaCliente == cliente.CedulaCliente)
                .SumAsync(v => v.PrecioTotal ?? 0);

            ViewBag.TotalCompras = totalCompras;
            ViewBag.TotalGastado = totalGastado;

            return View();
        }

        // GET: Cliente/Productos - Catálogo de productos
        public async Task<IActionResult> Productos(int? categoriaId, string buscar)
        {
            var productosQuery = _context.Productos
                .Include(p => p.IdCategoria)
                .Include(p => p.IdProveedorNavigation)
                .Where(p => p.CantidadDisponible > 0);

            // Filtrar por categoría
            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                productosQuery = productosQuery.Where(p => p.IdCategoria.Any(c => c.IdCategoria == categoriaId.Value));
            }

            // Buscar por nombre
            if (!string.IsNullOrEmpty(buscar))
            {
                productosQuery = productosQuery.Where(p => p.Nombre.Contains(buscar));
            }

            var productos = await productosQuery.ToListAsync();
            var categorias = await _context.Categoria.ToListAsync();

            ViewBag.Categorias = categorias;
            ViewBag.CategoriaSeleccionada = categoriaId;
            ViewBag.Buscar = buscar;

            return View(productos);
        }

        // GET: Cliente/MisCompras - Historial de compras
        public async Task<IActionResult> MisCompras()
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (cliente == null)
            {
                return RedirectToAction("Index");
            }

            var ventas = await _context.Venta
                .Include(v => v.CedulaEmpleadoNavigation)
                .Include(v => v.ProductoVenta)
                    .ThenInclude(pv => pv.IdProductoNavigation)
                .Where(v => v.CedulaCliente == cliente.CedulaCliente)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return View(ventas);
        }

        // GET: Cliente/DetalleCompra/5 - Detalle de una compra específica
        public async Task<IActionResult> DetalleCompra(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (cliente == null)
            {
                return RedirectToAction("Index");
            }

            var venta = await _context.Venta
                .Include(v => v.CedulaEmpleadoNavigation)
                .Include(v => v.CedulaClienteNavigation)
                .Include(v => v.ProductoVenta)
                    .ThenInclude(pv => pv.IdProductoNavigation)
                .Include(v => v.MetodoPagos)
                .Include(v => v.Facturas)
                .Where(v => v.IdVenta == id && v.CedulaCliente == cliente.CedulaCliente)
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Cliente/MiPerfil - Ver y editar perfil
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            var cliente = await _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (cliente == null)
            {
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // POST: Cliente/MiPerfil - Actualizar perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(Cliente cliente)
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var clienteActual = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);

            if (clienteActual == null)
            {
                return RedirectToAction("Index");
            }

            // Actualizar solo los campos permitidos
            clienteActual.Telefono = cliente.Telefono;
            clienteActual.Direccion = cliente.Direccion;
            clienteActual.Email = cliente.Email;

            try
            {
                _context.Update(clienteActual);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Perfil actualizado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar el perfil: " + ex.Message;
            }

            return RedirectToAction("MiPerfil");
        }
    }
}   