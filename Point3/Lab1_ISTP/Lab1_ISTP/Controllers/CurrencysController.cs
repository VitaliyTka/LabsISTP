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
    public class CurrencysController : Controller
    {
        private readonly DBPerfumerContext _context;

        public CurrencysController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: Currencys
        public async Task<IActionResult> Index()
        {
            return View(await _context.Currencys.ToListAsync());
        }

        // GET: Currencys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencys = await _context.Currencys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currencys == null)
            {
                return NotFound();
            }

            //return View(currencys);
            return RedirectToAction("Index", "Prices", new { id = currencys.Id, currency = currencys.Currency});
        }

        // GET: Currencys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Currency")] Currencys currencys)
        {
            if (ModelState.IsValid)
            {
                _context.Add(currencys);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currencys);
        }

        // GET: Currencys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencys = await _context.Currencys.FindAsync(id);
            if (currencys == null)
            {
                return NotFound();
            }
            return View(currencys);
        }

        // POST: Currencys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Currency")] Currencys currencys)
        {
            if (id != currencys.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currencys);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencysExists(currencys.Id))
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
            return View(currencys);
        }

        // GET: Currencys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currencys = await _context.Currencys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currencys == null)
            {
                return NotFound();
            }

            return View(currencys);
        }

        // POST: Currencys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currencys = await _context.Currencys.FindAsync(id);
            _context.Currencys.Remove(currencys);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencysExists(int id)
        {
            return _context.Currencys.Any(e => e.Id == id);
        }
    }
}
