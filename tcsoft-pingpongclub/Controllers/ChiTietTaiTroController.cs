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
	[ServiceFilter(typeof(MenuActionFilter))]
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
			var thuctapKtktcn2024Context = await _context.Sponors
								.Include(s => s.IdSponorNavigation)
								.Include(s => s.IdTournamentNavigation)
								.Where(s => s.Status == false)
								.OrderByDescending(e => e.IdSponorTour)
								.ToListAsync(); 

			var model = thuctapKtktcn2024Context.Select(e => new Sponor
			{
				IdSponorTour = e.IdSponorTour,
				IdSponor = e.IdSponor,
				IdSponorNavigation = e.IdSponorNavigation,
				IdTournament = e.IdTournament,
				IdTournamentNavigation = e.IdTournamentNavigation,
				Money = e.Money,
				Other = e.Other,
				CreatedDate = e.CreatedDate,
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
			ViewData["Type"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Thu" }, new SelectListItem { Value = "true", Text = "Chi" } };
			ViewData["IdSponor"] = new SelectList(_context.NhaTaiTros.Select(f => new { IdSponor = f.IdSponor, Display = f.NameSponer }), "IdSponor", "Display");
			ViewData["IdTournament"] = new SelectList(_context.Tournaments.Select(f => new { IdTournament = f.IdTournament, Display = f.TournamentName + ": " + (f.TimeStart.HasValue ? f.TimeStart.Value.ToString("dd/MM/yyyy") : "Không xác định") + " - " + (f.TimeEnd.HasValue ? f.TimeEnd.Value.ToString("dd/MM/yyyy") : "Không xác định") }), "IdTournament", "Display");
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
		public async Task<IActionResult> Create([Bind("IdSponorTour,Money,IdTournament,Status,IdSponor,Other,CreatedDate")] Sponor sponor)
		{
			if (ModelState.IsValid)
			{
				var newRecord = new Sponor
				{
					Money = sponor.Money,
					IdTournament = sponor.IdTournament,
					IdSponor = sponor.IdSponor,
					Other = sponor.Other,
					CreatedDate = sponor.CreatedDate,
					Status = false
				};

				_context.Add(newRecord);
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
			getData();
			return View(sponor);
		}

		// POST: ChiTietTaiTro/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdSponorTour,Money,IdTournament,Status,IdSponor,Other,CreatedDate")] Sponor sponor)
		{
			if (id != sponor.IdSponorTour)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var existingRecord = await _context.Sponors
						.Include(e => e.IdTournamentNavigation)
						.Include(e => e.IdSponorNavigation)
						.FirstOrDefaultAsync(e => e.IdSponorTour == sponor.IdSponorTour);

					existingRecord.Money = sponor.Money;
					existingRecord.IdTournament = sponor.IdTournament;
					existingRecord.IdSponor = sponor.IdSponor;
					existingRecord.Other = sponor.Other;
					existingRecord.CreatedDate = sponor.CreatedDate;
					sponor.Status = false;
					
					_context.Update(existingRecord);
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
			getData();
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
