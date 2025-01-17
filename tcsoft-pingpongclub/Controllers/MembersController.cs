using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using tcsoft_pingpongclub.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace tcsoft_pingpongclub.Controllers
{
	[ServiceFilter(typeof(MenuActionFilter))]
	public class MembersController : Controller
	{

		private readonly ThuctapKtktcn2024Context _context;
		private readonly IWebHostEnvironment environment;

		public MembersController(ThuctapKtktcn2024Context context, IWebHostEnvironment environment)
		{
			_context = context;
			this.environment = environment;
		}
		public ActionResult Index(int? page)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			int pageSize = 10;
			int pageNumber = page ?? 1;
			var members = _context.Members.Include(n => n.IdLevelNavigation).Include(n => n.IdRoleNavigation).Where(m => m.Status.Value);
			var paginatedList = members.ToPagedList(pageNumber, pageSize);
			return View(paginatedList);
		}
		public async Task<IActionResult> ResetPassword(int id)
		{

			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var editMemberPass = _context.Members.Find(id);
			var passwordHasher = new PasswordHasher<Member>();
			string defaultPassword = "1";
			editMemberPass.Password = passwordHasher.HashPassword(editMemberPass, defaultPassword);
			_context.Members.Update(editMemberPass);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public IActionResult Create()
		{
			ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "LevelName").Prepend(new SelectListItem { Text = "--Chọn mức rank--", Value = "" });
			ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "NameRole").Prepend(new SelectListItem { Text = "--Chọn chức vụ--", Value = "" });
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Member member, string Password)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var isEmailExist = await _context.Members.AnyAsync(m => m.Emaill == member.Emaill);
			var isUsernameExist = await _context.Members.AnyAsync(m => m.Username == member.Username);
			var score = _context.Levels.Find(member.IdLevel);
			if (isEmailExist)
			{
				ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
			}

			if (isUsernameExist)
			{
				ModelState.AddModelError("Username", "Tên người dùng đã tồn tại trong hệ thống.");
			}
			if (member.ImageFile != null)
			{
				string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
									 Path.GetExtension(member.ImageFile.FileName);

				string imageFolderPath = Path.Combine(environment.WebRootPath, "image");

				string imageFullPath = Path.Combine(imageFolderPath, newFileName);
				member.LinkAvatar = "/image/" + newFileName;
				using (var stream = System.IO.File.Create(imageFullPath))
				{
					await member.ImageFile.CopyToAsync(stream);
				}
			}
			else
			{
				ModelState.AddModelError("ImageFile", "Vui lòng chọn ảnh đại diện !");
			}
			if (ModelState.IsValid)
			{
				var passwordHasher = new PasswordHasher<Member>();
				member.Password = passwordHasher.HashPassword(member, Password);
				member.Status = true;
				_context.Add(member);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "LevelName").Prepend(new SelectListItem { Text = "--Chọn mức rank--", Value = "" });
			ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "NameRole").Prepend(new SelectListItem { Text = "--Chọn chức vụ--", Value = "" });
			return View(member);
		}

		// GET: Members/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (id == null)
			{
				return NotFound();
			}

			var member = await _context.Members.FindAsync(id);
			if (member == null)
			{
				return NotFound();
			}
			ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "LevelName", member.IdLevel).Prepend(new SelectListItem { Text = "--Chọn mức rank--", Value = "" });
			ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "NameRole", member.IdRole).Prepend(new SelectListItem { Text = "--Chọn chức vụ--", Value = "" });
			return View(member);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, IFormFile? ImageFile, string MemberName, string Address, string Phone, string Emaill, bool Gender, int IdLevel, int IdRole)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var editmember = _context.Members.Find(id);

			if (editmember == null)
			{
				return NotFound();
			}

			if (ImageFile != null)
			{
				string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(ImageFile.FileName);
				string imageFullPath = Path.Combine(environment.WebRootPath, "image", newFileName);
				using (var stream = System.IO.File.Create(imageFullPath))
				{
					await ImageFile.CopyToAsync(stream);
				}
				if (!string.IsNullOrEmpty(editmember.LinkAvatar))
				{
					string oldImgFullPath = Path.Combine(environment.WebRootPath, editmember.LinkAvatar.TrimStart('/'));
					if (System.IO.File.Exists(oldImgFullPath))
					{
						System.IO.File.Delete(oldImgFullPath);
					}
				}
				editmember.LinkAvatar = "/image/" + newFileName;
			}
			if (ModelState.IsValid)
			{
				editmember.MemberName = MemberName;
				editmember.Address = Address;
				editmember.Phone = Phone;
				editmember.Emaill = Emaill;
				editmember.Gender = Gender;
				editmember.IdLevel = IdLevel;
				editmember.IdRole = IdRole;
				_context.Update(editmember);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}
			ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "LevelName", editmember.IdLevel).Prepend(new SelectListItem { Text = "--Chọn mức rank--", Value = "" });
			ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "NameRole", editmember.IdRole).Prepend(new SelectListItem { Text = "--Chọn chức vụ--", Value = "" });
			return View(editmember);
		}
		public async Task<IActionResult> Delete(int? id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			if (id == null)
			{
				return NotFound();
			}

			var member = await _context.Members
				.Include(m => m.IdLevelNavigation)
				.Include(m => m.IdRoleNavigation)
				.FirstOrDefaultAsync(m => m.IdMember == id);
			if (member == null)
			{
				return NotFound();
			}

			return View(member);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			var member = _context.Members.Find(id);
			member.Status = false;
			_context.Members.Update(member);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool MemberExists(int id)
		{
			var isLoggedIn = HttpContext.Session.GetInt32("IdMember") != null;
			ViewBag.IsLoggedIn = isLoggedIn;
			return _context.Members.Any(e => e.IdMember == id);
		}
	}
}