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
    public class MetodoPagosController : Controller
    {
        private readonly CashblockContext _context;

        public MetodoPagosController(CashblockContext context)
        {
            _context = context;
        }

        // GET: MetodoPagos
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.MetodoPagos.Include(m => m.IdVentaNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: MetodoPagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoPago = await _context.MetodoPagos
                .Include(m => m.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdMetodo == id);
            if (metodoPago == null)
            {
                return NotFound();
            }

            return View(metodoPago);
        }

        // GET: MetodoPagos/Create
        public IActionResult Create()
        {
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta");
            return View();
        }

        // POST: MetodoPagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMetodo,TipoMetodo,NombreMetodo,IdVenta")] MetodoPago metodoPago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metodoPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", metodoPago.IdVenta);
            return View(metodoPago);
        }

        // GET: MetodoPagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoPago = await _context.MetodoPagos.FindAsync(id);
            if (metodoPago == null)
            {
                return NotFound();
            }
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", metodoPago.IdVenta);
            return View(metodoPago);
        }

        // POST: MetodoPagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMetodo,TipoMetodo,NombreMetodo,IdVenta")] MetodoPago metodoPago)
        {
            if (id != metodoPago.IdMetodo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metodoPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetodoPagoExists(metodoPago.IdMetodo))
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
            ViewData["IdVenta"] = new SelectList(_context.Venta, "IdVenta", "IdVenta", metodoPago.IdVenta);
            return View(metodoPago);
        }

        // GET: MetodoPagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoPago = await _context.MetodoPagos
                .Include(m => m.IdVentaNavigation)
                .FirstOrDefaultAsync(m => m.IdMetodo == id);
            if (metodoPago == null)
            {
                return NotFound();
            }

            return View(metodoPago);
        }

        // POST: MetodoPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metodoPago = await _context.MetodoPagos.FindAsync(id);
            if (metodoPago != null)
            {
                _context.MetodoPagos.Remove(metodoPago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetodoPagoExists(int id)
        {
            return _context.MetodoPagos.Any(e => e.IdMetodo == id);
        }
    }
}
