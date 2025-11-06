using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CashBlockApp.Models.Entities;
using ClosedXML.Excel;
using System.Drawing;
using AspNetCoreGeneratedDocument;
using CashBlockApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashBlockApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly CashblockContext _context;

        public UsuariosController(CashblockContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.Usuarios.Include(u => u.IdPerfilesNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdPerfilesNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "IdPerfiles");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreUsuario,Contraseña,IdPerfiles")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "IdPerfiles", usuario.IdPerfiles);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "IdPerfiles", usuario.IdPerfiles);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario,Contraseña,IdPerfiles")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            ViewData["IdPerfiles"] = new SelectList(_context.Perfiles, "IdPerfiles", "IdPerfiles", usuario.IdPerfiles);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdPerfilesNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
        // POST: Usuarios/Reporte
        public async Task<IActionResult> Reporte()
        {
            // Primero obtenemos todos los usuarios con sus relaciones
            var usuarios = await _context.Usuarios
                .Include(u => u.IdPerfilesNavigation)
                .Include(u => u.Clientes)
                    .ThenInclude(c => c.Venta)
                        .ThenInclude(v => v.ProductoVenta)
                            .ThenInclude(pv => pv.IdProductoNavigation)
                .Include(u => u.Empleados)
                    .ThenInclude(e => e.Venta)
                        .ThenInclude(v => v.ProductoVenta)
                            .ThenInclude(pv => pv.IdProductoNavigation)
                .ToListAsync();

            // Luego procesamos en memoria
            var reporte = usuarios.Select(u =>
            {
                var ventasCliente = u.Clientes.SelectMany(c => c.Venta).ToList();
                var ventasEmpleado = u.Empleados.SelectMany(e => e.Venta).ToList();

                var productosCliente = ventasCliente
                    .SelectMany(v => v.ProductoVenta)
                    .Select(pv => pv.IdProductoNavigation?.Nombre)
                    .Where(n => n != null)
                    .Distinct()
                    .ToList();

                var productosVendidos = ventasEmpleado
                    .SelectMany(v => v.ProductoVenta)
                    .Select(pv => pv.IdProductoNavigation?.Nombre)
                    .Where(n => n != null)
                    .Distinct()
                    .ToList();

                return new ReporteUsuarioViewModel
                {
                    NombreUsuario = u.NombreUsuario,
                    Rol = u.IdPerfilesNavigation?.Rol ?? "Sin rol",

                    // Si es Cliente
                    TotalPedidos = ventasCliente.Count,
                    Productos = productosCliente.Any() ? string.Join(", ", productosCliente) : "Sin productos",
                    MontoTotal = ventasCliente.Sum(v => v.PrecioTotal ?? 0),

                    // Si es Empleado (Vendedor)
                    TotalVentas = ventasEmpleado.Count,
                    ProductosVendidos = productosVendidos.Any() ? string.Join(", ", productosVendidos) : "Sin productos",
                    MontoVendido = ventasEmpleado.Sum(v => v.PrecioTotal ?? 0)
                };
            }).ToList();

            return View(reporte);
        }

        // GET: Usuarios/ExportarExcel
        public async Task<IActionResult> ExportarExcel()
        {
            // Primero obtenemos todos los usuarios con sus relaciones
            var usuarios = await _context.Usuarios
                .Include(u => u.IdPerfilesNavigation)
                .Include(u => u.Clientes)
                    .ThenInclude(c => c.Venta)
                        .ThenInclude(v => v.ProductoVenta)
                            .ThenInclude(pv => pv.IdProductoNavigation)
                .Include(u => u.Empleados)
                    .ThenInclude(e => e.Venta)
                        .ThenInclude(v => v.ProductoVenta)
                            .ThenInclude(pv => pv.IdProductoNavigation)
                .ToListAsync();

            // Luego procesamos en memoria
            var reporte = usuarios.Select(u =>
            {
                var ventasCliente = u.Clientes.SelectMany(c => c.Venta).ToList();
                var ventasEmpleado = u.Empleados.SelectMany(e => e.Venta).ToList();

                var productosCliente = ventasCliente
                    .SelectMany(v => v.ProductoVenta)
                    .Select(pv => pv.IdProductoNavigation?.Nombre)
                    .Where(n => n != null)
                    .Distinct()
                    .ToList();

                var productosVendidos = ventasEmpleado
                    .SelectMany(v => v.ProductoVenta)
                    .Select(pv => pv.IdProductoNavigation?.Nombre)
                    .Where(n => n != null)
                    .Distinct()
                    .ToList();

                return new ReporteUsuarioViewModel
                {
                    NombreUsuario = u.NombreUsuario,
                    Rol = u.IdPerfilesNavigation?.Rol ?? "Sin rol",

                    // Si es Cliente
                    TotalPedidos = ventasCliente.Count,
                    Productos = productosCliente.Any() ? string.Join(", ", productosCliente) : "Sin productos",
                    MontoTotal = ventasCliente.Sum(v => v.PrecioTotal ?? 0),

                    // Si es Empleado (Vendedor)
                    TotalVentas = ventasEmpleado.Count,
                    ProductosVendidos = productosVendidos.Any() ? string.Join(", ", productosVendidos) : "Sin productos",
                    MontoVendido = ventasEmpleado.Sum(v => v.PrecioTotal ?? 0)
                };
            }).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Usuarios");
                var filas = 1;

                // Encabezados
                worksheet.Cell(filas, 1).Value = "Usuario";
                worksheet.Cell(filas, 2).Value = "Rol";
                worksheet.Cell(filas, 3).Value = "Total Pedidos/Ventas";
                worksheet.Cell(filas, 4).Value = "Productos";
                worksheet.Cell(filas, 5).Value = "Monto Total";

                // Aplicar estilo a encabezados
                var headerRange = worksheet.Range(filas, 1, filas, 5);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Datos
                foreach (var item in reporte)
                {
                    filas++;
                    worksheet.Cell(filas, 1).Value = item.NombreUsuario;
                    worksheet.Cell(filas, 2).Value = item.Rol;

                    // Si es Cliente
                    if (item.TotalPedidos > 0)
                    {
                        worksheet.Cell(filas, 3).Value = item.TotalPedidos;
                        worksheet.Cell(filas, 4).Value = item.Productos;
                        worksheet.Cell(filas, 5).Value = item.MontoTotal;
                    }
                    // Si es Vendedor
                    else if (item.TotalVentas > 0)
                    {
                        worksheet.Cell(filas, 3).Value = item.TotalVentas;
                        worksheet.Cell(filas, 4).Value = item.ProductosVendidos;
                        worksheet.Cell(filas, 5).Value = item.MontoVendido;
                    }
                    else
                    {
                        worksheet.Cell(filas, 3).Value = 0;
                        worksheet.Cell(filas, 4).Value = "Sin actividad";
                        worksheet.Cell(filas, 5).Value = 0;
                    }

                    // Formato de moneda para la columna 5
                    worksheet.Cell(filas, 5).Style.NumberFormat.Format = "$#,##0.00";
                }

                // Ajustar columnas automáticamente
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var contenido = stream.ToArray();
                    return File(contenido,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"ReporteUsuarios_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                }
            }
        }
    }
}

