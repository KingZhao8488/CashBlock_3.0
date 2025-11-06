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
    public class ExistenciasController : Controller
    {
        private readonly CashblockContext _context;

        public ExistenciasController(CashblockContext context)
        {
            _context = context;
        }

        // GET: Existencias
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.Existencias.Include(e => e.CedulaEmpleadoNavigation).Include(e => e.IdAlmacenNavigation).Include(e => e.IdProductoNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: Existencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existencia = await _context.Existencias
                .Include(e => e.CedulaEmpleadoNavigation)
                .Include(e => e.IdAlmacenNavigation)
                .Include(e => e.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdExistencias == id);
            if (existencia == null)
            {
                return NotFound();
            }

            return View(existencia);
        }

        // GET: Existencias/Create
        public IActionResult Create()
        {
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado");
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: Existencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExistencias,CantidadAlmacen,CedulaEmpleado,IdAlmacen,IdProducto")] Existencia existencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(existencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", existencia.CedulaEmpleado);
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", existencia.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", existencia.IdProducto);
            return View(existencia);
        }

        // GET: Existencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existencia = await _context.Existencias.FindAsync(id);
            if (existencia == null)
            {
                return NotFound();
            }
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", existencia.CedulaEmpleado);
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", existencia.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", existencia.IdProducto);
            return View(existencia);
        }

        // POST: Existencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExistencias,CantidadAlmacen,CedulaEmpleado,IdAlmacen,IdProducto")] Existencia existencia)
        {
            if (id != existencia.IdExistencias)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExistenciaExists(existencia.IdExistencias))
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
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", existencia.CedulaEmpleado);
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", existencia.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", existencia.IdProducto);
            return View(existencia);
        }

        // GET: Existencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existencia = await _context.Existencias
                .Include(e => e.CedulaEmpleadoNavigation)
                .Include(e => e.IdAlmacenNavigation)
                .Include(e => e.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdExistencias == id);
            if (existencia == null)
            {
                return NotFound();
            }

            return View(existencia);
        }

        // POST: Existencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existencia = await _context.Existencias.FindAsync(id);
            if (existencia != null)
            {
                _context.Existencias.Remove(existencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExistenciaExists(int id)
        {
            return _context.Existencias.Any(e => e.IdExistencias == id);
        }
    }
}
