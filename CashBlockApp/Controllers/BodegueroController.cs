using CashBlockApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CashBlockApp.Data;

namespace CashBlockApp.Controllers
{
    public class BodegueroController : Controller
    {
        private readonly CashblockContext _context;

        public BodegueroController(CashblockContext context)
        {
            _context = context;
        }

        // GET: /Bodeguero/Inventario
        [HttpGet]
        public async Task<IActionResult> Inventario()
        {
            var inventario = await _context.Existencias
                .Include(e => e.IdProductoNavigation)
                .Include(e => e.IdAlmacenNavigation)
                .Select(e => new
                {
                    CodigoProducto = e.IdProducto.ToString(),
                    NombreProducto = e.IdProductoNavigation.Nombre,

                    CantidadDisponible = e.CantidadAlmacen,

                    IdAlmacen = e.IdAlmacen,
                    UbicacionAlmacen = e.IdAlmacen.ToString(),

                    IdProducto = e.IdProducto
                })
                .OrderBy(i => i.IdAlmacen)
                .ThenBy(i => i.NombreProducto)
                .ToListAsync();

            return View(inventario);
        }

        // POST: /Bodeguero/GuardarStock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarStock(int IdProducto, int IdAlmacen, int CantidadAlmacen)
        {
            var existenciaOriginal = await _context.Existencias
                .FirstOrDefaultAsync(e => e.IdProducto == IdProducto && e.IdAlmacen == IdAlmacen);

            if (existenciaOriginal == null)
            {
                TempData["Error"] = "Error: El registro de existencia no fue encontrado.";
                return RedirectToAction(nameof(Inventario));
            }

            existenciaOriginal.CantidadAlmacen = CantidadAlmacen;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existenciaOriginal);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Stock actualizado: {IdProducto} en almacén {IdAlmacen}. Nueva cantidad: {existenciaOriginal.CantidadAlmacen}";
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "Error de concurrencia al actualizar el stock.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al guardar: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "El valor del stock no es válido.";
            }

            return RedirectToAction(nameof(Inventario));
        }
    }
}