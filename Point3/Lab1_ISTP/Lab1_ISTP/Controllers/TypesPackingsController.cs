using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_ISTP;

namespace Lab1_ISTP.Controllers
{
    public class TypesPackingsController : Controller
    {
        private readonly DBPerfumerContext _context;

        public TypesPackingsController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: TypesPackings
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypesPackings.ToListAsync());
        }

        // GET: TypesPackings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesPackings = await _context.TypesPackings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesPackings == null)
            {
                return NotFound();
            }

            /*return View(typesPackings);*/
            return RedirectToAction("Index", "Packings", new { typesPackingId = typesPackings.Id, typesPacking = typesPackings.TypePacking });
        }

        // GET: TypesPackings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypesPackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypePacking")] TypesPackings typesPackings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typesPackings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typesPackings);
        }

        // GET: TypesPackings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesPackings = await _context.TypesPackings.FindAsync(id);
            if (typesPackings == null)
            {
                return NotFound();
            }
            return View(typesPackings);
        }

        // POST: TypesPackings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypePacking")] TypesPackings typesPackings)
        {
            if (id != typesPackings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typesPackings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesPackingsExists(typesPackings.Id))
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
            return View(typesPackings);
        }

        // GET: TypesPackings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesPackings = await _context.TypesPackings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesPackings == null)
            {
                return NotFound();
            }

            return View(typesPackings);
        }

        // POST: TypesPackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typesPackings = await _context.TypesPackings.FindAsync(id);
            _context.TypesPackings.Remove(typesPackings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesPackingsExists(int id)
        {
            return _context.TypesPackings.Any(e => e.Id == id);
        }
    }
}
