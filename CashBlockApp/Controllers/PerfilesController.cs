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
    public class PerfilesController : Controller
    {
        private readonly CashblockContext _context;

        public PerfilesController(CashblockContext context)
        {
            _context = context;
        }

        // GET: Perfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Perfiles.ToListAsync());
        }

        // GET: Perfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfiles = await _context.Perfiles
                .FirstOrDefaultAsync(m => m.IdPerfiles == id);
            if (perfiles == null)
            {
                return NotFound();
            }

            return View(perfiles);
        }

        // GET: Perfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Perfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPerfiles,Rol,Descripcion")] Perfiles perfiles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(perfiles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(perfiles);
        }

        // GET: Perfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfiles = await _context.Perfiles.FindAsync(id);
            if (perfiles == null)
            {
                return NotFound();
            }
            return View(perfiles);
        }

        // POST: Perfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerfiles,Rol,Descripcion")] Perfiles perfiles)
        {
            if (id != perfiles.IdPerfiles)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfiles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilesExists(perfiles.IdPerfiles))
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
            return View(perfiles);
        }

        // GET: Perfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfiles = await _context.Perfiles
                .FirstOrDefaultAsync(m => m.IdPerfiles == id);
            if (perfiles == null)
            {
                return NotFound();
            }

            return View(perfiles);
        }

        // POST: Perfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfiles = await _context.Perfiles.FindAsync(id);
            if (perfiles != null)
            {
                _context.Perfiles.Remove(perfiles);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilesExists(int id)
        {
            return _context.Perfiles.Any(e => e.IdPerfiles == id);
        }
    }
}
