using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CashBlockApp.Models.Entities;

namespace CashBlockApp.Controllers
{
    public class ProductoVentasController : Controller
    {
        private readonly CashblockContext _context;

        public ProductoVentasController(CashblockContext context)
        {
            _context = context;
        }

        // GET: ProductoVentas
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.ProductoVenta.Include(p => p.IdProductoNavigation).Include(p => p.IdVentaNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: ProductoVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoVenta = await _context.ProductoVenta
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoVenta == null)
            {
                return NotFound();
            }

            return View(productoVenta);
        }

        // GET: ProductoVentas/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta");
            return View();
        }

        // POST: ProductoVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdVenta,CantidadVendida")] ProductoVenta productoVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoVenta.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", productoVenta.IdVenta);
            return View(productoVenta);
        }

        // GET: ProductoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoVenta = await _context.ProductoVenta.FindAsync(id);
            if (productoVenta == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoVenta.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", productoVenta.IdVenta);
            return View(productoVenta);
        }

        // POST: ProductoVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdVenta,CantidadVendida")] ProductoVenta productoVenta)
        {
            if (id != productoVenta.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoVentaExists(productoVenta.IdProducto))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoVenta.IdProducto);
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", productoVenta.IdVenta);
            return View(productoVenta);
        }

        // GET: ProductoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoVenta = await _context.ProductoVenta
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoVenta == null)
            {
                return NotFound();
            }

            return View(productoVenta);
        }

        // POST: ProductoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoVenta = await _context.ProductoVenta.FindAsync(id);
            if (productoVenta != null)
            {
                _context.ProductoVenta.Remove(productoVenta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoVentaExists(int id)
        {
            return _context.ProductoVenta.Any(e => e.IdProducto == id);
        }
    }
}
