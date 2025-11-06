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
    public class VentasController : Controller
    {
        private readonly CashblockContext _context;

        public VentasController(CashblockContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var cashblockContext = _context.Venta.Include(v => v.CedulaClienteNavigation).Include(v => v.CedulaEmpleadoNavigation);
            return View(await cashblockContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.CedulaClienteNavigation)
                .Include(v => v.CedulaEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "CedulaCliente", "CedulaCliente");
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,TotalProductos,FechaVenta,PrecioTotal,CedulaEmpleado,CedulaCliente")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "CedulaCliente", "CedulaCliente", venta.CedulaCliente);
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", venta.CedulaEmpleado);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "CedulaCliente", "CedulaCliente", venta.CedulaCliente);
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", venta.CedulaEmpleado);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,TotalProductos,FechaVenta,PrecioTotal,CedulaEmpleado,CedulaCliente")] Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta))
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
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "CedulaCliente", "CedulaCliente", venta.CedulaCliente);
            ViewData["CedulaEmpleado"] = new SelectList(_context.Empleados, "CedulaEmpleado", "CedulaEmpleado", venta.CedulaEmpleado);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.CedulaClienteNavigation)
                .Include(v => v.CedulaEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta != null)
            {
                _context.Venta.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Venta.Any(e => e.IdVenta == id);
        }
    }
}
