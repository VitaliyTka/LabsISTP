using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_ISTP;

using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP.Controllers
{
    public class PerfumesController : Controller
    {
        private readonly DBPerfumerContext _context;

        public PerfumesController(DBPerfumerContext context)
        {
            _context = context;
        }
        //id = prices.Id, price = prices.Price, data = prices.DateCreation ,currency = prices.Currency
        // GET: Perfumes
        public async Task<IActionResult> Index(int? priceId, int? price, int? packingId, int? volume)
        {
            if(priceId != null)
            {
                ViewBag.Check = "price";
                ViewBag.Id = priceId;
                ViewBag.Data = " з ціною " + price;
                var PerfumeByPrice = _context.Perfumes
                    .Where(b => b.PriceId == priceId)
                    .Include(b => b.Packing.TypePacking)
                    .Include(b => b.Price.Currency)
                    .Include(b => b.Packing.PerfumeInformation)
                    .Include(b => b.Packing.PerfumeInformation.ClassificationPerfume)
                    .Include(b => b.Packing.PerfumeInformation.Manufacturer);

                return View(await PerfumeByPrice.ToListAsync());
            }
            if(packingId != null)
            {
                ViewBag.Check = "packing";
                ViewBag.Id = packingId;
                ViewBag.Data = " з Об'ємом " + volume;
                var PerfumeByPrice = _context.Perfumes
                    .Where(b => b.PackingId == packingId)
                    .Include(b => b.Packing.TypePacking)
                    .Include(b => b.Price.Currency)
                    .Include(b => b.Packing.PerfumeInformation)
                    .Include(b => b.Packing.PerfumeInformation.ClassificationPerfume)
                    .Include(b => b.Packing.PerfumeInformation.Manufacturer);

                return View(await PerfumeByPrice.ToListAsync());

            }
            // var dBPerfumerContext = _context.Perfumes.Include(p => p.Packing).Include(p => p.Price);

            ViewBag.Check = "";
            var AllPerfume = _context.Perfumes
                .Include(b => b.Packing.TypePacking)
                .Include(b => b.Price.Currency)
                .Include(b => b.Packing.PerfumeInformation)
                .Include(b => b.Packing.PerfumeInformation.ClassificationPerfume)
                .Include(b => b.Packing.PerfumeInformation.Manufacturer);
            return View(await AllPerfume.ToListAsync());
        }

        // GET: Perfumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumes = await _context.Perfumes
                .Include(p => p.Packing)
                .Include(p => p.Price)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfumes == null)
            {
                return NotFound();
            }

            return View(perfumes);
        }

        // GET: Perfumes/Create
        public IActionResult Create(int id, string check)
        {
            Console.WriteLine("id ============== " + id);
            ViewBag.Check = check;
            if (check == "price")
            {
                ViewData["PackingId"] = new SelectList(_context.Packings, "Id", "Volume");
                ViewBag.InputId = id;
                ViewBag.Data = "ціною " + _context.Prices
                    .Where(c => c.Id == id)
                    .FirstOrDefault().Price;

                return View();
            }
            if (check == "packing")
            {
                ViewData["PriceId"] = new SelectList(_context.Prices, "Id", "Price");
                ViewBag.InputId = id;
                ViewBag.Data = "об'ємом " + _context.Packings
                    .Where(c => c.Id == id)
                    .FirstOrDefault().Volume.ToString();

                return View();
            }
            ViewBag.Check = "";
            ViewData["PackingId"] = new SelectList(_context.Packings, "Id", "Volume");
            ViewData["PriceId"] = new SelectList(_context.Prices, "Id", "Price");
            return View();
        }

        // POST: Perfumes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string check, [Bind("PackingId,PriceId")] Perfumes perfumes)
        {
            Console.WriteLine("check : " + check);
            Console.WriteLine("PackingId : " + perfumes.PackingId);
            Console.WriteLine("PriceId : " + perfumes.PriceId);
            Console.WriteLine("Id : " + perfumes.Id);
            if (check == "price")
            {
                perfumes.PriceId = id;
                if (ModelState.IsValid)
                {
                    _context.Add(perfumes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Perfumes", new
                    {
                        priceId = perfumes.PriceId,
                        price = _context.Prices
                            .Where(c => c.Id == perfumes.PriceId)
                            .FirstOrDefault().Price
                    });
                }
            }
            if(check == "packing")
            {
                perfumes.PackingId = id;

                if (ModelState.IsValid)
                {
                    _context.Add(perfumes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Perfumes", new
                    {
                        packingId = perfumes.PackingId,
                        volume = _context.Packings
                            .Where(c => c.Id == perfumes.PackingId)
                            .FirstOrDefault().Volume
                    });
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(perfumes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackingId"] = new SelectList(_context.Perfumes, "Id", "Volume", perfumes.PackingId);
            ViewData["PriceId"] = new SelectList(_context.Perfumes, "Id", "Price", perfumes.PriceId);
            return View(perfumes);
        }

        // GET: Perfumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumes = await _context.Perfumes.FindAsync(id);
            if (perfumes == null)
            {
                return NotFound();
            }
            ViewData["PackingId"] = new SelectList(_context.Packings, "Id", "Volume", perfumes.PackingId);
            ViewData["PriceId"] = new SelectList(_context.Prices, "Id", "Price", perfumes.PriceId);
            return View(perfumes);
        }

        // POST: Perfumes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PackingId,PriceId")] Perfumes perfumes)
        {
            if (id != perfumes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfumes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfumesExists(perfumes.Id))
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
            ViewData["PackingId"] = new SelectList(_context.Packings, "Id", "Id", perfumes.PackingId);
            ViewData["PriceId"] = new SelectList(_context.Prices, "Id", "Id", perfumes.PriceId);
            return View(perfumes);
        }

        // GET: Perfumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumes = await _context.Perfumes
                .Include(p => p.Packing)
                .Include(p => p.Price)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfumes == null)
            {
                return NotFound();
            }

            return View(perfumes);
        }

        // POST: Perfumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfumes = await _context.Perfumes.FindAsync(id);
            _context.Perfumes.Remove(perfumes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfumesExists(int id)
        {
            return _context.Perfumes.Any(e => e.Id == id);
        }
    }
}
