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
    public class RecepcionMercanciasController : Controller
    {
        private readonly CashblockContext _context;

        public RecepcionMercanciasController(CashblockContext context)
        {
            _context = context;
        }

        // GET: RecepcionMercancias
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.RecepcionMercancia.Include(r => r.IdAlmacenNavigation).Include(r => r.IdOrdenNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: RecepcionMercancias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionMercancia = await _context.RecepcionMercancia
                .Include(r => r.IdAlmacenNavigation)
                .Include(r => r.IdOrdenNavigation)
                .FirstOrDefaultAsync(m => m.IdRecepcion == id);
            if (recepcionMercancia == null)
            {
                return NotFound();
            }

            return View(recepcionMercancia);
        }

        // GET: RecepcionMercancias/Create
        public IActionResult Create()
        {
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen");
            ViewData["IdOrden"] = new SelectList(_context.OrdenCompras, "IdOrden", "IdOrden");
            return View();
        }

        // POST: RecepcionMercancias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRecepcion,CantidadRecibida,FechaRecepcion,IdOrden,IdAlmacen")] RecepcionMercancia recepcionMercancia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recepcionMercancia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", recepcionMercancia.IdAlmacen);
            ViewData["IdOrden"] = new SelectList(_context.OrdenCompras, "IdOrden", "IdOrden", recepcionMercancia.IdOrden);
            return View(recepcionMercancia);
        }

        // GET: RecepcionMercancias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionMercancia = await _context.RecepcionMercancia.FindAsync(id);
            if (recepcionMercancia == null)
            {
                return NotFound();
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", recepcionMercancia.IdAlmacen);
            ViewData["IdOrden"] = new SelectList(_context.OrdenCompras, "IdOrden", "IdOrden", recepcionMercancia.IdOrden);
            return View(recepcionMercancia);
        }

        // POST: RecepcionMercancias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRecepcion,CantidadRecibida,FechaRecepcion,IdOrden,IdAlmacen")] RecepcionMercancia recepcionMercancia)
        {
            if (id != recepcionMercancia.IdRecepcion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recepcionMercancia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionMercanciaExists(recepcionMercancia.IdRecepcion))
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
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", recepcionMercancia.IdAlmacen);
            ViewData["IdOrden"] = new SelectList(_context.OrdenCompras, "IdOrden", "IdOrden", recepcionMercancia.IdOrden);
            return View(recepcionMercancia);
        }

        // GET: RecepcionMercancias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionMercancia = await _context.RecepcionMercancia
                .Include(r => r.IdAlmacenNavigation)
                .Include(r => r.IdOrdenNavigation)
                .FirstOrDefaultAsync(m => m.IdRecepcion == id);
            if (recepcionMercancia == null)
            {
                return NotFound();
            }

            return View(recepcionMercancia);
        }

        // POST: RecepcionMercancias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionMercancia = await _context.RecepcionMercancia.FindAsync(id);
            if (recepcionMercancia != null)
            {
                _context.RecepcionMercancia.Remove(recepcionMercancia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionMercanciaExists(int id)
        {
            return _context.RecepcionMercancia.Any(e => e.IdRecepcion == id);
        }
    }
}
