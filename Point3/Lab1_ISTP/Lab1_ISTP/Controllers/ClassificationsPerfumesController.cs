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
    public class ClassificationsPerfumesController : Controller
    {
        private readonly DBPerfumerContext _context;

        public ClassificationsPerfumesController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: ClassificationsPerfumes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClassificationsPerfumes.ToListAsync());
        }

        // GET: ClassificationsPerfumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classificationsPerfumes = await _context.ClassificationsPerfumes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classificationsPerfumes == null)
            {
                return NotFound();
            }

            //return View(classificationsPerfumes);
            return RedirectToAction("Index", "PerfumesInformations", new { 
                classificationsPerfumeId = classificationsPerfumes.Id, 
                classificationsPerfume = classificationsPerfumes.ClassificationPerfume 
            });
        }

        // GET: ClassificationsPerfumes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClassificationsPerfumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassificationPerfume")] ClassificationsPerfumes classificationsPerfumes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classificationsPerfumes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classificationsPerfumes);
        }

        // GET: ClassificationsPerfumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classificationsPerfumes = await _context.ClassificationsPerfumes.FindAsync(id);
            if (classificationsPerfumes == null)
            {
                return NotFound();
            }
            return View(classificationsPerfumes);
        }

        // POST: ClassificationsPerfumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassificationPerfume")] ClassificationsPerfumes classificationsPerfumes)
        {
            if (id != classificationsPerfumes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classificationsPerfumes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassificationsPerfumesExists(classificationsPerfumes.Id))
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
            return View(classificationsPerfumes);
        }

        // GET: ClassificationsPerfumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classificationsPerfumes = await _context.ClassificationsPerfumes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classificationsPerfumes == null)
            {
                return NotFound();
            }

            return View(classificationsPerfumes);
        }

        // POST: ClassificationsPerfumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classificationsPerfumes = await _context.ClassificationsPerfumes.FindAsync(id);
            _context.ClassificationsPerfumes.Remove(classificationsPerfumes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassificationsPerfumesExists(int id)
        {
            return _context.ClassificationsPerfumes.Any(e => e.Id == id);
        }
    }
}
