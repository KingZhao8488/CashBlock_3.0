using CashBlockApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CashBlockApp.Controllers
{
    public class VendedorController : Controller
    {
        private readonly CashblockContext _context;

        public VendedorController(CashblockContext context)
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
            else if (perfilId != 2) // Solo vendedores
            {
                context.Result = RedirectToAction("Index", "Home");
            }

            base.OnActionExecuting(context);
        }

        // GET: Vendedor/Index - Dashboard del vendedor
        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");

            var empleado = await _context.Empleados
                .Include(e => e.IdUsuarioNavigation)
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioId);

            if (empleado == null)
            {
                ViewBag.Error = "No se encontró información del vendedor";
                return View();
            }

            ViewBag.NombreVendedor = $"{empleado.Nombre} {empleado.Apellido}";
            ViewBag.CedulaVendedor = empleado.CedulaEmpleado;

            // Estadísticas del vendedor
            var totalVentas = await _context.Venta
                .Where(v => v.CedulaEmpleado == empleado.CedulaEmpleado)
                .CountAsync();

            var totalVendido = await _context.Venta
                .Where(v => v.CedulaEmpleado == empleado.CedulaEmpleado)
                .SumAsync(v => v.PrecioTotal ?? 0);

            var ventasHoy = await _context.Venta
                .Where(v => v.CedulaEmpleado == empleado.CedulaEmpleado &&
                            v.FechaVenta == DateOnly.FromDateTime(DateTime.Now))
                .CountAsync();

            ViewBag.TotalVentas = totalVentas;
            ViewBag.TotalVendido = totalVendido;
            ViewBag.VentasHoy = ventasHoy;

            return View();
        }

        // GET: Vendedor/Productos - Ver productos disponibles
        public async Task<IActionResult> Productos(int? categoriaId, string buscar)
        {
            var productosQuery = _context.Productos
                .Include(p => p.IdCategoria)
                .Include(p => p.IdProveedorNavigation)
                .AsQueryable();

            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                productosQuery = productosQuery.Where(p => p.IdCategoria.Any(c => c.IdCategoria == categoriaId.Value));
            }

            if (!string.IsNullOrEmpty(buscar))
            {
                productosQuery = productosQuery.Where(p => p.Nombre.Contains(buscar));
            }

            var productos = await productosQuery.OrderBy(p => p.Nombre).ToListAsync();
            var categorias = await _context.Categoria.ToListAsync();

            ViewBag.Categorias = categorias;
            ViewBag.CategoriaSeleccionada = categoriaId;
            ViewBag.Buscar = buscar;

            return View(productos);
        }

        // GET: Vendedor/NuevaVenta - Formulario para registrar venta
        public async Task<IActionResult> NuevaVenta()
        {
            var clientes = await _context.Clientes
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var productos = await _context.Productos
                .Where(p => p.CantidadDisponible > 0)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Clientes = clientes;
            ViewBag.Productos = productos;

            return View();
        }

        // POST: Vendedor/RegistrarVenta - Procesar venta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVenta(string cedulaCliente, List<int> productosIds, List<int> cantidades, string tipoMetodo, string nombreMetodo)
        {
            if (string.IsNullOrEmpty(cedulaCliente) || productosIds == null || productosIds.Count == 0)
            {
                TempData["Error"] = "Debe seleccionar un cliente y al menos un producto";
                return RedirectToAction("NuevaVenta");
            }

            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioId);

            if (empleado == null)
            {
                TempData["Error"] = "No se encontró información del vendedor";
                return RedirectToAction("Index");
            }

            try
            {
                // Crear la venta
                var venta = new Venta
                {
                    CedulaCliente = cedulaCliente,
                    CedulaEmpleado = empleado.CedulaEmpleado,
                    FechaVenta = DateOnly.FromDateTime(DateTime.Now),
                    TotalProductos = cantidades.Sum(),
                    PrecioTotal = 0
                };

                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();

                decimal totalVenta = 0;
                int totalProductos = 0;

                // Agregar productos a la venta
                for (int i = 0; i < productosIds.Count; i++)
                {
                    if (cantidades[i] > 0)
                    {
                        var producto = await _context.Productos.FindAsync(productosIds[i]);

                        if (producto != null && producto.CantidadDisponible >= cantidades[i])
                        {
                            var productoVenta = new ProductoVenta
                            {
                                IdVenta = venta.IdVenta,
                                IdProducto = productosIds[i],
                                CantidadVendida = cantidades[i]
                            };

                            _context.ProductoVenta.Add(productoVenta);

                            // Actualizar stock
                            producto.CantidadDisponible -= cantidades[i];
                            _context.Update(producto);

                            totalVenta += producto.PrecioVenta * cantidades[i];
                            totalProductos += cantidades[i];
                        }
                    }
                }

                // Actualizar totales de la venta
                venta.PrecioTotal = totalVenta;
                venta.TotalProductos = totalProductos;
                _context.Update(venta);

                // Crear método de pago
                var metodoPago = new MetodoPago
                {
                    IdVenta = venta.IdVenta,
                    TipoMetodo = tipoMetodo,
                    NombreMetodo = nombreMetodo
                };
                _context.MetodoPagos.Add(metodoPago);

                // Crear factura
                var factura = new Factura
                {
                    IdVenta = venta.IdVenta,
                    PrecioTotal = totalVenta,
                    FechaVenta = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Facturas.Add(factura);

                await _context.SaveChangesAsync();

                TempData["Success"] = $"Venta registrada exitosamente. Total: ${totalVenta:N2}";
                return RedirectToAction("DetalleVenta", new { id = venta.IdVenta });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar la venta: " + ex.Message;
                return RedirectToAction("NuevaVenta");
            }
        }

        // GET: Vendedor/MisVentas - Historial de ventas del vendedor
        public async Task<IActionResult> MisVentas()
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioId);

            if (empleado == null)
            {
                return RedirectToAction("Index");
            }

            var ventas = await _context.Venta
                .Include(v => v.CedulaClienteNavigation)
                .Include(v => v.ProductoVenta)
                    .ThenInclude(pv => pv.IdProductoNavigation)
                .Where(v => v.CedulaEmpleado == empleado.CedulaEmpleado)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return View(ventas);
        }

        // GET: Vendedor/DetalleVenta/5 - Ver detalle de una venta
        public async Task<IActionResult> DetalleVenta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.IdUsuario == usuarioId);

            if (empleado == null)
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
                .Where(v => v.IdVenta == id && v.CedulaEmpleado == empleado.CedulaEmpleado)
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Vendedor/ObtenerPrecioProducto - API para obtener precio (AJAX)
        [HttpGet]
        public async Task<IActionResult> ObtenerPrecioProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                precio = producto.PrecioVenta,
                stock = producto.CantidadDisponible,
                nombre = producto.Nombre
            });
        }
    }
}