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
	public class ExpenseAndIncomeController : Controller
	{
		private readonly ThuctapKtktcn2024Context _context;

		public ExpenseAndIncomeController(ThuctapKtktcn2024Context context)
		{
			_context = context;
		}

		// GET: ExpenseAndIncome
		public async Task<IActionResult> Index()
		{
			String? url = HttpContext.Session.GetString("url");
			if (url == "exspenserole")
			{
				var thuctapKtktcn2024Context = _context.ExpenseAndIncomes.Include(e => e.IdFundNavigation).Include(e => e.IdPartyNavigation).Include(e => e.IdReasonNavigation);
				return View(await thuctapKtktcn2024Context.ToListAsync());

			}
			else
			{
				return RedirectToAction("Index", "Login");
			}

		}

		public async Task<IActionResult> GetSponsors()
		{
			var sponsors = await _context.NhaTaiTros.ToListAsync();
			return View(sponsors);
		}

		// GET: ExpenseAndIncome/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var expenseAndIncome = await _context.ExpenseAndIncomes
				.Include(e => e.IdFundNavigation)
				.Include(e => e.IdPartyNavigation)
				.Include(e => e.IdReasonNavigation)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (expenseAndIncome == null)
			{
				return NotFound();
			}

			return View(expenseAndIncome);
		}
		// GET: ExpenseAndIncome/Create
		public IActionResult Create()
		{
			//ViewData["IdFund"] = new SelectList(_context.Funds, "IdFund", "IdFund");
			ViewData["IdFund"] = new SelectList(_context.Funds.Select(f => new { IdFund = f.IdFund, Display = f.FundName }), "IdFund", "Display");
			ViewData["IdParty"] = new SelectList(_context.Members.Select(f => new { IdParty = f.IdMember, Display = f.MemberName }), "IdParty", "Display");
			//ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason");
			ViewData["IdReason"] = new SelectList(_context.Reasons.Select(f => new { IdReason = f.IdReason, Display = f.ReasonName }), "IdReason", "Display");
			return View();
		}

		// POST: ExpenseAndIncome/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdFund,IdParty,IdAccountant,Type,IdReason,DaysOverdue,Status,IsDone")] ExpenseAndIncome expenseAndIncome)
		{
			if (ModelState.IsValid)
			{
				// Gán giá trị mặc định
				expenseAndIncome.IsDone = expenseAndIncome.IsDone ?? false;
				expenseAndIncome.DaysOverdue = expenseAndIncome.DaysOverdue ?? 0;
				expenseAndIncome.Status = expenseAndIncome.Status ?? false;

				_context.Add(expenseAndIncome);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["IdFund"] = new SelectList(_context.Funds, "IdFund", "IdFund", expenseAndIncome.IdFund);
			ViewData["IdParty"] = new SelectList(_context.Members, "IdMember", "IdMember", expenseAndIncome.IdParty);
			ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", expenseAndIncome.IdReason);
			return View(expenseAndIncome);
		}


		// GET: ExpenseAndIncome/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var expenseAndIncome = await _context.ExpenseAndIncomes.FindAsync(id);
			if (expenseAndIncome == null)
			{
				return NotFound();
			}
			ViewData["IdFund"] = new SelectList(_context.Funds, "IdFund", "IdFund", expenseAndIncome.IdFund);
			ViewData["IdParty"] = new SelectList(_context.Members, "IdMember", "IdMember", expenseAndIncome.IdParty);
			ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", expenseAndIncome.IdReason);
			return View(expenseAndIncome);
		}

		// POST: ExpenseAndIncome/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,IdFund,IdParty,IdAccountant,IsDone,Type,IdReason,DaysOverdue,Status")] ExpenseAndIncome expenseAndIncome)
		{
			if (id != expenseAndIncome.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(expenseAndIncome);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ExpenseAndIncomeExists(expenseAndIncome.Id))
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
			ViewData["IdFund"] = new SelectList(_context.Funds, "IdFund", "IdFund", expenseAndIncome.IdFund);
			ViewData["IdParty"] = new SelectList(_context.Members, "IdMember", "IdMember", expenseAndIncome.IdParty);
			ViewData["IdReason"] = new SelectList(_context.Reasons, "IdReason", "IdReason", expenseAndIncome.IdReason);
			return View(expenseAndIncome);
		}

		// GET: ExpenseAndIncome/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var expenseAndIncome = await _context.ExpenseAndIncomes
				.Include(e => e.IdFundNavigation)
				.Include(e => e.IdPartyNavigation)
				.Include(e => e.IdReasonNavigation)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (expenseAndIncome == null)
			{
				return NotFound();
			}

			return View(expenseAndIncome);
		}

		// POST: ExpenseAndIncome/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var expenseAndIncome = await _context.ExpenseAndIncomes.FindAsync(id);
			if (expenseAndIncome != null)
			{
				_context.ExpenseAndIncomes.Remove(expenseAndIncome);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ExpenseAndIncomeExists(int id)
		{
			return _context.ExpenseAndIncomes.Any(e => e.Id == id);
		}
	}
}
