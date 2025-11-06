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
    public class ProductoComprasController : Controller
    {
        private readonly CashblockContext _context;

        public ProductoComprasController(CashblockContext context)
        {
            _context = context;
        }

        // GET: ProductoCompras
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.ProductoCompras.Include(p => p.IdCompraNavigation).Include(p => p.IdProductoNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: ProductoCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras
                .Include(p => p.IdCompraNavigation)
                .Include(p => p.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCompra == null)
            {
                return NotFound();
            }

            return View(productoCompra);
        }

        // GET: ProductoCompras/Create
        public IActionResult Create()
        {
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: ProductoCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdCompra,CantidadComprada")] ProductoCompra productoCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", productoCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoCompra.IdProducto);
            return View(productoCompra);
        }

        // GET: ProductoCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras.FindAsync(id);
            if (productoCompra == null)
            {
                return NotFound();
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", productoCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoCompra.IdProducto);
            return View(productoCompra);
        }

        // POST: ProductoCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdCompra,CantidadComprada")] ProductoCompra productoCompra)
        {
            if (id != productoCompra.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoCompraExists(productoCompra.IdProducto))
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
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", productoCompra.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", productoCompra.IdProducto);
            return View(productoCompra);
        }

        // GET: ProductoCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras
                .Include(p => p.IdCompraNavigation)
                .Include(p => p.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCompra == null)
            {
                return NotFound();
            }

            return View(productoCompra);
        }

        // POST: ProductoCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoCompra = await _context.ProductoCompras.FindAsync(id);
            if (productoCompra != null)
            {
                _context.ProductoCompras.Remove(productoCompra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoCompraExists(int id)
        {
            return _context.ProductoCompras.Any(e => e.IdProducto == id);
        }
    }
}
