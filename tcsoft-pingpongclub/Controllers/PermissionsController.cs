using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Filter;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
	[ServiceFilter(typeof(MenuActionFilter))]
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class PermissionsController : Controller
	{
		private readonly ThuctapKtktcn2024Context _context;

		public PermissionsController(ThuctapKtktcn2024Context context)
		{
			_context = context;
		}

		// GET: Permissions
		public async Task<IActionResult> Index()
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			return View(await _context.Permissions.ToListAsync());
		}

		// GET: Permissions/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			// Kiểm tra id có null không
			if (id == null)
			{
				return NotFound("ID không hợp lệ.");
			}

			// Tìm quyền trong cơ sở dữ liệu
			var permission = await _context.Permissions
				   .Include(p => p.ParentPermission)
				   .FirstOrDefaultAsync(m => m.IdPermission == id);

			// Kiểm tra xem quyền có tồn tại hay không
			if (permission == null)
			{
				return NotFound("Quyền không tồn tại.");
			}

			// Trả về View với đối tượng permission tìm thấy
			return View(permission);
		}


		// GET: Permissions/Create
		public async Task<IActionResult> Create()
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var lstPermission = await _context.Permissions.Where(p => p.IdPerParent == null).ToListAsync();
			ViewBag.Permission = lstPermission;
			return View();
		}


		// POST: Permissions/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("IdPermission,NamePermission,Url,IdPerParent,Status")] Permission permission)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (ModelState.IsValid)
			{
				// Kiểm tra quyền cha có hợp lệ hay không
				if (permission.IdPerParent.HasValue)
				{
					var parentPermission = await _context.Permissions.FindAsync(permission.IdPerParent.Value);
					if (parentPermission == null)
					{
						ModelState.AddModelError("IdPerParent", "Quyền cha không hợp lệ.");
						return View(permission);
					}
				}

				_context.Add(permission);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(permission);
		}


		// GET: Permissions/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (id == null)
			{
				return NotFound();
			}

			var permission = _context.Permissions
				.FirstOrDefault(p => p.IdPermission == id);

			if (permission == null)
			{
				return NotFound();
			}

			// Đưa danh sách quyền vào ViewBag để hiển thị trong dropdown
			ViewBag.Permissions = _context.Permissions.Where(p => p.IdPerParent == null || p.IdPermission == permission.IdPermission).ToList();

			return View(permission);
		}


		// POST: Permissions/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IdPermission,NamePermission,Url,IdPerParent,Status")] Permission permission)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (id != permission.IdPermission)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					if (permission.IdPermission == permission.IdPerParent)
					{
						permission.IdPerParent = null;
					}
					_context.Update(permission);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PermissionExists(permission.IdPermission))
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
			ViewBag.Permissions = _context.Permissions.Where(p => p.IdPerParent == null || p.IdPermission == permission.IdPermission).ToList();
			return View(permission);
		}

		// GET: Permissions/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (id == null)
			{
				return NotFound();
			}

			var permission = await _context.Permissions
				   .Include(p => p.ParentPermission)
				   .FirstOrDefaultAsync(m => m.IdPermission == id);
			if (permission == null)
			{
				return NotFound();
			}

			return View(permission);
		}

		// POST: Permissions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var permission = await _context.Permissions.FindAsync(id);
			if (permission != null)
			{
				_context.Permissions.Remove(permission);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool PermissionExists(int id)
		{
			return _context.Permissions.Any(e => e.IdPermission == id);
		}
	}
}
