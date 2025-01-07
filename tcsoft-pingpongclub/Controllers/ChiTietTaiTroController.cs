using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class ChiTietTaiTroController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public ChiTietTaiTroController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: ChiTietTaiTro
        public async Task<IActionResult> Index()
        {
            var thuctapKtktcn2024Context = await _context.Sponors.Include(s => s.IdIncomeNavigation)
                                .Include(s => s.IdSponorNavigation)
                                .Include(s => s.IdTournamentNavigation)
                                .Where(s => s.Status == false)
                                .OrderByDescending(e => e.IdSponorTour)
                                .ToListAsync(); ;

            var model = thuctapKtktcn2024Context.Select(e => new Sponor
            {
                IdSponorTour = e.IdSponorTour,
                IdIncome = e.IdIncome,
                IdIncomeNavigation = e.IdIncomeNavigation,
                IdSponor = e.IdSponor,
                IdSponorNavigation = e.IdSponorNavigation,
                IdTournament = e.IdTournament,
                IdTournamentNavigation = e.IdTournamentNavigation,
                Money = e.Money,
                Other = e.Other,
                Status = e.Status
            }).ToList();

            return View(model);
        }

        // GET: ChiTietTaiTro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponor = await _context.Sponors
                .Include(s => s.IdIncomeNavigation)
                .Include(s => s.IdSponorNavigation)
                .Include(s => s.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdSponorTour == id);
            if (sponor == null)
            {
                return NotFound();
            }

            return View(sponor);
        }
        public void getData()
        {
            ViewData["IdIncome"] = new SelectList(_context.ExpenseAndIncomes.Where(f => f.Type == true).Select(f => new { IdIncome = f.Id, Display = f.IdReasonNavigation.ReasonName + " - " + (f.CreatedDate.HasValue ? f.CreatedDate.Value.ToString("dd/MM/yyyy") : "Không xác định") }),"IdIncome","Display");
            ViewData["IdSponor"] = new SelectList(_context.NhaTaiTros.Select(f => new { IdSponor = f.IdSponor, Display = f.NameSponer }), "IdSponor", "Display");
            ViewData["IdTournament"] = new SelectList(_context.Tournaments.Select(f => new { IdTournament = f.IdTournament, Display = f.TournamentName + " ~ Bắt đầu: " + (f.TimeStart.HasValue ? f.TimeStart.Value.ToString("dd/MM/yyyy") : "Không xác định") + " - Kết thúc: " + (f.TimeEnd.HasValue ? f.TimeEnd.Value.ToString("dd/MM/yyyy") : "Không xác định") }), "IdTournament", "Display");
        }

        // GET: ChiTietTaiTro/Create
        public IActionResult Create()
        {
            getData();
            return View();
        }

        // POST: ChiTietTaiTro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSponorTour,IdIncome,Money,IdTournament,Status,IdSponor,Other")] Sponor sponor)
        {
            if (ModelState.IsValid)
            {

                _context.Add(sponor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            getData();
            return View(sponor);
        }

        // GET: ChiTietTaiTro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponor = await _context.Sponors.FindAsync(id);
            if (sponor == null)
            {
                return NotFound();
            }
            ViewData["IdIncome"] = new SelectList(_context.ExpenseAndIncomes, "Id", "Id", sponor.IdIncome);
            ViewData["IdSponor"] = new SelectList(_context.NhaTaiTros, "IdSponor", "IdSponor", sponor.IdSponor);
            ViewData["IdTournament"] = new SelectList(_context.Tournaments, "IdTournament", "IdTournament", sponor.IdTournament);
            return View(sponor);
        }

        // POST: ChiTietTaiTro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSponorTour,IdIncome,Money,IdTournament,Status,IdSponor,Other")] Sponor sponor)
        {
            if (id != sponor.IdSponorTour)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponorExists(sponor.IdSponorTour))
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
            ViewData["IdIncome"] = new SelectList(_context.ExpenseAndIncomes, "Id", "Id", sponor.IdIncome);
            ViewData["IdSponor"] = new SelectList(_context.NhaTaiTros, "IdSponor", "IdSponor", sponor.IdSponor);
            ViewData["IdTournament"] = new SelectList(_context.Tournaments, "IdTournament", "IdTournament", sponor.IdTournament);
            return View(sponor);
        }

        // GET: ChiTietTaiTro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponor = await _context.Sponors
                .Include(s => s.IdIncomeNavigation)
                .Include(s => s.IdSponorNavigation)
                .Include(s => s.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdSponorTour == id);
            if (sponor == null)
            {
                return NotFound();
            }

            return View(sponor);
        }

        // POST: ChiTietTaiTro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponor = await _context.Sponors.FindAsync(id);
            if (sponor != null)
            {
                _context.Sponors.Remove(sponor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponorExists(int id)
        {
            return _context.Sponors.Any(e => e.IdSponorTour == id);
        }
    }
}
