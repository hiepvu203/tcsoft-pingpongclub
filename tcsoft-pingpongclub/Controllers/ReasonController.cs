using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Service;
using tcsoft_pingpongclub.Filter;
namespace tcsoft_pingpongclub.Controllers
{
	[ServiceFilter(typeof(MenuActionFilter))]
	
	public class ReasonController : Controller
	{
		private readonly ThuctapKtktcn2024Context _context;

		public ReasonController(ThuctapKtktcn2024Context context)
		{
			_context = context;
		}

		// GET: Reason
		public async Task<IActionResult> Index()
		{
			return View(await _context.Reasons.Where(e => e.Status == false).ToListAsync());
		}

		// GET: Reason/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reason = await _context.Reasons
				.FirstOrDefaultAsync(m => m.IdReason == id);
			if (reason == null)
			{
				return NotFound();
			}

			return View(reason);
		}

		public void getData()
		{
			ViewData["IdFund"] = new SelectList(_context.Funds.Select(f => new { IdFund = f.IdFund, Display = f.FundName }), "IdFund", "Display");
			ViewData["IdAccountant"] = new SelectList(_context.Members.Select(f => new { IdAccountant = f.IdMember, Display = f.MemberName }), "IdAccountant", "Display");
			ViewData["IdParty"] = new SelectList(_context.Members.Select(f => new { IdParty = f.IdMember, Display = f.MemberName }), "IdParty", "Display");
			ViewData["IdReason"] = new SelectList(_context.Reasons.Select(f => new { IdReason = f.IdReason, Display = f.ReasonName }), "IdReason", "Display");
			ViewData["Type"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Thu" }, new SelectListItem { Value = "true", Text = "Chi" } };
			ViewData["IsDone"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Chưa hoàn thành" }, new SelectListItem { Value = "true", Text = "Hoàn thành" } };
		}

		// GET: Reason/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Reason/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdReason,ReasonName,Type,Status")] Reason reason)
		{
			if (ModelState.IsValid)
			{
				_context.Add(reason);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(reason);
		}

		// GET: Reason/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reason = await _context.Reasons.FindAsync(id);
			if (reason == null)
			{
				return NotFound();
			}
			getData();
			return View(reason);
		}

		// POST: Reason/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdReason,ReasonName,Type,Status")] Reason reason)
		{
			if (id != reason.IdReason)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(reason);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReasonExists(reason.IdReason))
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
			return View(reason);
		}

		// GET: Reason/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var reason = await _context.Reasons
				.FirstOrDefaultAsync(m => m.IdReason == id);
			if (reason == null)
			{
				return NotFound();
			}

			return View(reason);
		}

		// POST: Reason/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var reason = await _context.Reasons.FindAsync(id);
			if (reason != null)
			{
				reason.Status = true;
				_context.Update(reason);
				await _context.SaveChangesAsync();
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ReasonExists(int id)
		{
			return _context.Reasons.Any(e => e.IdReason == id);
		}
	}
}
