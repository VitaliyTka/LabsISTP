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
    public class PerfumesInformationsController : Controller
    {
        private readonly DBPerfumerContext _context;

        public PerfumesInformationsController(DBPerfumerContext context)
        {
            _context = context;
        }

        // GET: PerfumesInformations
        public async Task<IActionResult> Index(int? manufacturerId, string? manufacturer, int? classificationsPerfumeId, string? classificationsPerfume)
        {
            if(manufacturerId != null)
            {
                ViewBag.Check = "manufacturer";
                ViewBag.Id = manufacturerId;
                ViewBag.Data = "з виробником " + manufacturer;
                var PerfumesInformationByManufacturer = _context.PerfumesInformations
                    .Where(b => b.ManufacturerId == manufacturerId)
                    .Include(b => b.Manufacturer)
                    .Include(b => b.ClassificationPerfume);

                return View(await PerfumesInformationByManufacturer.ToListAsync());
            }
            if(classificationsPerfumeId != null)
            {
                ViewBag.Check = "classificationsPerfume";
                ViewBag.Id = classificationsPerfumeId;
                ViewBag.Data = "з типом парфуму " + classificationsPerfume;
                var PerfumesInformationByClassificationsPerfume = _context.PerfumesInformations
                    .Where(b => b.ClassificationPerfumeId == classificationsPerfumeId)
                    .Include(b => b.Manufacturer)
                    .Include(b => b.ClassificationPerfume);

                return View(await PerfumesInformationByClassificationsPerfume.ToListAsync());
            }
            ViewBag.Check = "";
            var PerfumesInformation = _context.PerfumesInformations
                .Include(b => b.Manufacturer)
                .Include(b => b.ClassificationPerfume);
            return View(await PerfumesInformation.ToListAsync());
        }

        // GET: PerfumesInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumesInformations = await _context.PerfumesInformations
                .Include(p => p.ClassificationPerfume)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfumesInformations == null)
            {
                return NotFound();
            }
            /*
                        return View(perfumesInformations);*/
            return RedirectToAction("Index", "Packings", new { perfumeInformationId = perfumesInformations.Id, perfumeName = perfumesInformations.PerfumeName});
        }

        // GET: PerfumesInformations/Create
        public IActionResult Create(int id, string check)
        {
            ViewBag.Check = check;
            if(check == "manufacturer")
            {
                ViewData["ClassificationPerfumeId"] = new SelectList(_context.ClassificationsPerfumes, "Id", "ClassificationPerfume");
                ViewBag.InputId = id;
                ViewBag.Data = "виробник " + _context.Manufacturers
                    .Where(c => c.Id == id)
                    .FirstOrDefault().Manufacturer;

                return View();
            }
            if(check == "classificationsPerfume")
            {
                ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Manufacturer");
                ViewBag.InputId = id;
                ViewBag.Data = "класифікація парфуму " + _context.ClassificationsPerfumes
                    .Where(c => c.Id == id)
                    .FirstOrDefault().ClassificationPerfume;

                return View();
            }
            ViewBag.Check = "";
            ViewData["ClassificationPerfumeId"] = new SelectList(_context.ClassificationsPerfumes, "Id", "ClassificationPerfume");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Manufacturer");
            return View();
        }

        // POST: PerfumesInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int id, 
            string check, 
            [Bind("Id,YearOfIssue,ClassificationPerfumeId,ManufacturerId,PicturePath,PerfumeName")] PerfumesInformations perfumesInformations)
        {
            Console.WriteLine("check : " + check);
            Console.WriteLine("YearOfIssue : " + perfumesInformations.YearOfIssue);
            Console.WriteLine("PicturePath : " + perfumesInformations.PicturePath);
            Console.WriteLine("PerfumeName : " + perfumesInformations.PerfumeName);
            Console.WriteLine("ManufacturerId : " + perfumesInformations.ManufacturerId);
            Console.WriteLine("ClassificationPerfumeId : " + perfumesInformations.ClassificationPerfumeId);
            Console.WriteLine("Id : " + id);
            if (check == "manufacturer")
            {
                perfumesInformations.ManufacturerId = id;
                if (ModelState.IsValid)
                {
                    _context.Add(perfumesInformations);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "PerfumesInformations", new
                    {
                        manufacturerId = perfumesInformations.ManufacturerId,
                        manufacturer = _context.Manufacturers
                            .Where(c => c.Id == perfumesInformations.ManufacturerId)
                            .FirstOrDefault().Manufacturer
                    });
                }
            }
            if (check == "classificationsPerfume")
            {
                perfumesInformations.ClassificationPerfumeId = id;
                if (ModelState.IsValid)
                {
                    _context.Add(perfumesInformations);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "PerfumesInformations", new
                    {
                        classificationsPerfumeId = perfumesInformations.ClassificationPerfumeId,
                        classificationsPerfume = _context.ClassificationsPerfumes
                            .Where(c => c.Id == perfumesInformations.ClassificationPerfumeId)
                            .FirstOrDefault().ClassificationPerfume
                    });
                }
            }

            ViewData["ClassificationPerfumeId"] = new SelectList(_context.ClassificationsPerfumes, "Id", "ClassificationPerfume", perfumesInformations.ClassificationPerfumeId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Manufacturer", perfumesInformations.ManufacturerId);
            return View(perfumesInformations);
        }

        // GET: PerfumesInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumesInformations = await _context.PerfumesInformations.FindAsync(id);
            if (perfumesInformations == null)
            {
                return NotFound();
            }
            ViewData["ClassificationPerfumeId"] = new SelectList(_context.ClassificationsPerfumes, "Id", "ClassificationPerfume", perfumesInformations.ClassificationPerfumeId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Manufacturer", perfumesInformations.ManufacturerId);
            return View(perfumesInformations);
        }

        // POST: PerfumesInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,YearOfIssue,ClassificationPerfumeId,ManufacturerId,PicturePath,PerfumeName")] PerfumesInformations perfumesInformations)
        {
            if (id != perfumesInformations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfumesInformations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfumesInformationsExists(perfumesInformations.Id))
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
            ViewData["ClassificationPerfumeId"] = new SelectList(_context.ClassificationsPerfumes, "Id", "ClassificationPerfume", perfumesInformations.ClassificationPerfumeId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Manufacturer", perfumesInformations.ManufacturerId);
            return View(perfumesInformations);
        }

        // GET: PerfumesInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfumesInformations = await _context.PerfumesInformations
                .Include(p => p.ClassificationPerfume)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfumesInformations == null)
            {
                return NotFound();
            }

            return View(perfumesInformations);
        }

        // POST: PerfumesInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfumesInformations = await _context.PerfumesInformations.FindAsync(id);
            _context.PerfumesInformations.Remove(perfumesInformations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfumesInformationsExists(int id)
        {
            return _context.PerfumesInformations.Any(e => e.Id == id);
        }
    }
}
