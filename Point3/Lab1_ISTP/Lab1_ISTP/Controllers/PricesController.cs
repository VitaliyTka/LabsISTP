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
    public class PricesController : Controller
    {
        private readonly DBPerfumerContext _context;

        public PricesController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: Prices
        public async Task<IActionResult> Index(int? id, string? currency)
        {
            //if (id == null) return RedirectToAction("Index", "Currencys");
            if (id != null)
            {
                ViewBag.CurrencysId = id;
                ViewBag.CurrencysCurrency = currency;
                var PriceByCurrency = _context.Prices.Where(b => b.CurrencyId == id).Include(b => b.Currency);
                //var dBPerfumerContext = _context.Prices.Include(p => p.Currency);
                return View(await PriceByCurrency.ToListAsync());
            }
            var dBPerfumerContext = _context.Prices.Include(p => p.Currency);
            return View(await dBPerfumerContext.ToListAsync());
        }

        // GET: Prices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prices = await _context.Prices
                .Include(p => p.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prices == null)
            {
                return NotFound();
            }

            //return View(prices);
            return RedirectToAction("Index", "Perfumes", new { priceId = prices.Id, price = prices.Price });
        }

        // GET: Prices/Create
        public IActionResult Create(int currencyId)
        {
            ViewBag.CurrencyId = currencyId;
            ViewBag.Currency = _context.Currencys
                .Where(c => c.Id == currencyId)
                .FirstOrDefault().Currency;
            //ViewData["CurrencyId"] = new SelectList(_context.Currencys, "Id", "Id");
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int currencyId, [Bind("Id,Price,DateCreation,CurrencyId")] Prices prices)
        {
            prices.DateCreation = DateTime.Now;
            prices.CurrencyId = currencyId;
            Console.WriteLine("IDDDD : " + prices.Id);
            if (ModelState.IsValid)
            {
                _context.Add(prices);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Prices", new 
                { 
                    id = currencyId,
                    currency = _context.Currencys
                        .Where(c => c.Id == currencyId)
                        .FirstOrDefault().Currency
                });
            }
            //ViewData["CurrencyId"] = new SelectList(_context.Currencys, "Id", "Id", prices.CurrencyId);
            return RedirectToAction("Index", "Prices", new
            {
                id = currencyId,
                currency = _context.Currencys
                    .Where(c => c.Id == currencyId)
                    .FirstOrDefault().Currency
            });
        }

        // GET: Prices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prices = await _context.Prices.FindAsync(id);
            if (prices == null)
            {
                return NotFound();
            }
            ViewData["CurrencyId"] = new SelectList(_context.Currencys, "Id", "Currency", prices.CurrencyId);
            return View(prices);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,DateCreation,CurrencyId")] Prices prices)
        {
            prices.DateCreation = DateTime.Now;
            if (id != prices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricesExists(prices.Id))
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
            ViewData["CurrencyId"] = new SelectList(_context.Currencys, "Id", "Id", prices.CurrencyId);
            return View(prices);
        }

        // GET: Prices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prices = await _context.Prices
                .Include(p => p.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prices == null)
            {
                return NotFound();
            }

            return View(prices);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prices = await _context.Prices.FindAsync(id);
            _context.Prices.Remove(prices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricesExists(int id)
        {
            return _context.Prices.Any(e => e.Id == id);
        }
    }
}
