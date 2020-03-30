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
    public class PackingsController : Controller
    {
        private readonly DBPerfumerContext _context;

        public PackingsController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: Packings
        public async Task<IActionResult> Index(int? typesPackingId, string? typesPacking, int? perfumeInformationId, string? perfumeName)
        {
            if(typesPackingId != null)
            {
                ViewBag.Check = "typesPacking";
                ViewBag.Id = typesPackingId;
                ViewBag.Data = "з типом пакунку " + typesPacking;
                var PackingByTypesPacking = _context.Packings
                    .Where(b => b.TypePackingId == typesPackingId)
                    .Include(b => b.TypePacking)
                    .Include(b => b.PerfumeInformation)
                    .Include(b => b.PerfumeInformation.ClassificationPerfume)
                    .Include(b => b.PerfumeInformation.Manufacturer);

                return View(await PackingByTypesPacking.ToListAsync());
            }
            if(perfumeInformationId != null)
            {
                ViewBag.Check = "perfumeInformation";
                ViewBag.Id = perfumeInformationId;
                ViewBag.Data = "з назвою парфуму " + perfumeName;
                var PackingByPerfumeInformation = _context.Packings
                    .Where(b => b.PerfumeInformationId == perfumeInformationId)
                    .Include(b => b.TypePacking)
                    .Include(b => b.PerfumeInformation)
                    .Include(b => b.PerfumeInformation.ClassificationPerfume)
                    .Include(b => b.PerfumeInformation.Manufacturer);

                return View(await PackingByPerfumeInformation.ToListAsync());
            }
            var dBPerfumerContext = _context.Packings
                .Include(p => p.PerfumeInformation)
                .Include(p => p.TypePacking)
                .Include(b => b.PerfumeInformation.ClassificationPerfume)
                .Include(b => b.PerfumeInformation.Manufacturer);

            return View(await dBPerfumerContext.ToListAsync());
        }

        // GET: Packings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packings = await _context.Packings
                .Include(p => p.PerfumeInformation)
                .Include(p => p.TypePacking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packings == null)
            {
                return NotFound();
            }

            //return View(packings);
            return RedirectToAction("Index", "Perfumes", new { packingId = packings.Id, volume = packings.Volume });
        }

        // GET: Packings/Create
        public IActionResult Create(int id, string check)
        {
            ViewBag.Check = check;
            Console.WriteLine("check : " + check);
            if (check == "typesPacking")
            {
                ViewData["PerfumeInformationId"] = new SelectList(_context.PerfumesInformations, "Id", "PerfumeName");
                ViewBag.InputId = id;
                ViewBag.Data = "класифікація парфуму " + _context.TypesPackings
                    .Where(c => c.Id == id)
                    .FirstOrDefault().TypePacking;

                return View();
            }
            if (check == "perfumeInformation")
            {
                ViewData["TypePackingId"] = new SelectList(_context.TypesPackings, "Id", "TypePacking");
                ViewBag.InputId = id;
                ViewBag.Data = "класифікація парфуму " + _context.PerfumesInformations
                    .Where(c => c.Id == id)
                    .FirstOrDefault().PerfumeName;

                return View();
            }
            ViewBag.Check = "";
            ViewData["PerfumeInformationId"] = new SelectList(_context.PerfumesInformations, "Id", "PerfumeName");
            ViewData["TypePackingId"] = new SelectList(_context.TypesPackings, "Id", "TypePacking");
            return View();
        }

        // POST: Packings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int id,
            string check,
            [Bind("TypePackingId,Volume,PerfumeInformationId")] Packings packings)
        {

            if (check == "typesPacking")
            {
                packings.TypePackingId = id;
                if (ModelState.IsValid)
                {
                    _context.Add(packings);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Packings", new
                    {
                        typesPackingId = packings.TypePackingId,
                        typesPacking = _context.TypesPackings
                            .Where(c => c.Id == packings.TypePackingId)
                            .FirstOrDefault().TypePacking
                    });
                }
            }

            if (check == "perfumeInformation")
            {
                packings.PerfumeInformationId = id;
                if (ModelState.IsValid)
                {
                    _context.Add(packings);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Packings", new
                    {
                        perfumeInformationId = packings.PerfumeInformationId,
                        perfumeName = _context.PerfumesInformations
                            .Where(c => c.Id == packings.PerfumeInformationId)
                            .FirstOrDefault().PerfumeName
                    });
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(packings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PerfumeInformationId"] = new SelectList(_context.PerfumesInformations, "Id", "Id", packings.PerfumeInformationId);
            ViewData["TypePackingId"] = new SelectList(_context.TypesPackings, "Id", "Id", packings.TypePackingId);
            return View(packings);
        }

        // GET: Packings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packings = await _context.Packings.FindAsync(id);
            if (packings == null)
            {
                return NotFound();
            }
            ViewData["PerfumeInformationId"] = new SelectList(_context.PerfumesInformations, "Id", "PerfumeName", packings.PerfumeInformationId);
            ViewData["TypePackingId"] = new SelectList(_context.TypesPackings, "Id", "TypePacking", packings.TypePackingId);
            return View(packings);
        }

        // POST: Packings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypePackingId,Volume,PerfumeInformationId")] Packings packings)
        {
            if (id != packings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(packings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackingsExists(packings.Id))
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
            ViewData["PerfumeInformationId"] = new SelectList(_context.PerfumesInformations, "Id", "Id", packings.PerfumeInformationId);
            ViewData["TypePackingId"] = new SelectList(_context.TypesPackings, "Id", "Id", packings.TypePackingId);
            return View(packings);
        }

        // GET: Packings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packings = await _context.Packings
                .Include(p => p.PerfumeInformation)
                .Include(p => p.TypePacking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (packings == null)
            {
                return NotFound();
            }

            return View(packings);
        }

        // POST: Packings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packings = await _context.Packings.FindAsync(id);
            _context.Packings.Remove(packings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackingsExists(int id)
        {
            return _context.Packings.Any(e => e.Id == id);
        }
    }
}
