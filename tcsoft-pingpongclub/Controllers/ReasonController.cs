using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using X.PagedList;
using X.PagedList.Extensions;

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
		public async Task<IActionResult> Index(int? page)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			int pageSize = 5;
			int pageNumber = page ?? 1;

			var reasons = await _context.Reasons
				.Where(e => e.Status == false)
				.ToListAsync();

			var paginatedList = reasons.ToPagedList(pageNumber, pageSize);

			return View(paginatedList);
		}


		// GET: Reason/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
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
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			ViewData["Type"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Thu" }, new SelectListItem { Value = "true", Text = "Chi" } };
			ViewData["recurringFee"] = new List<SelectListItem> { new SelectListItem { Value = "false", Text = "Không" }, new SelectListItem { Value = "true", Text = "Có" } };
		}

		// GET: Reason/Create
		public IActionResult Create()
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			getData();
			return View();
		}

		// POST: Reason/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdReason,ReasonName,Type,Status,recurringFee")] Reason reason)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (ModelState.IsValid)
			{
				_context.Add(reason);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			getData();
			return View(reason);
		}

		// GET: Reason/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
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
		public async Task<IActionResult> Edit(int id, [Bind("IdReason,ReasonName,Type,Status,recurringFee")] Reason reason)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
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
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
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
			getData();
			return View(reason);
		}

		// POST: Reason/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
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
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			return _context.Reasons.Any(e => e.IdReason == id);
		}
	}
}