using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.Filter;
using tcsoft_pingpongclub.Service;
namespace tcsoft_pingpongclub.Controllers
{
	[ServiceFilter(typeof(MenuActionFilter))]
	[ServiceFilter(typeof(AuthorizationFilter))]
	public class UserController : Controller
	{
		private readonly ThuctapKtktcn2024Context _context;


		public UserController(ThuctapKtktcn2024Context context)
		{
			_context = context;
		}
		// GET: Userr/Index
		public async Task<IActionResult> Index()
		{

			// Lấy IdMember từ Session
			int? idMember = HttpContext.Session.GetInt32("IdMember");
			// Tìm kiếm thông tin người dùng từ bảng Members với IdMember = 2
			var member = await _context.Members
				.Include(m => m.IdLevelNavigation)
				.Include(m => m.IdRoleNavigation)
				.FirstOrDefaultAsync(m => m.IdMember == idMember);

			if (member == null)
			{
				return RedirectToAction("Index"); // Nếu không tìm thấy người dùng, chuyển hướng đến trang đăng nhập
			}

			return View(member); // Trả về thông tin của người dùng cho view
		}

		// GET: Userr/Edit
		public async Task<IActionResult> Edit()
		{
			int? idMember = HttpContext.Session.GetInt32("IdMember");

			// Lấy thông tin người dùng từ bảng Members với IdMember = 3
			var member = await _context.Members.FindAsync(idMember);
			if (member == null)
			{
				TempData["Error"] = "Không tìm thấy người dùng!";
				return RedirectToAction("Index");
			}

			// Trả về thông tin người dùng cho view
			ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "LevelName", member.IdLevel);
			ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "RoleName", member.IdRole);
			return View(member);
		}


		// POST: Userr/Edit
		[Route("Userr/Edit/{id}")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("IdMember,MemberName,Address,Phone,Emaill,Gender,LinkAvatar, Password")] Member member, IFormFile LinkAvatarFile)
		{
			int? id = HttpContext.Session.GetInt32("IdMember");
			if (id != member.IdMember)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				// Ghi lại lỗi nếu ModelState không hợp lệ
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				foreach (var error in errors)
				{
					Console.WriteLine(error.ErrorMessage);
				}
				return View(member);
			}

			try
			{
				// Lấy dữ liệu cũ từ cơ sở dữ liệu
				var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.IdMember == id);
				if (existingMember == null)
				{
					return NotFound();
				}
				existingMember.MemberName = member.MemberName;
				existingMember.Address = member.Address;
				existingMember.Phone = member.Phone;
				existingMember.Emaill = member.Emaill;
				existingMember.Gender = member.Gender;


				// Xử lý file ảnh nếu có
				if (LinkAvatarFile != null && LinkAvatarFile.Length > 0)
				{
					var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
					Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại

					// Xóa ảnh cũ nếu tồn tại
					if (!string.IsNullOrEmpty(existingMember.LinkAvatar))
					{
						var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingMember.LinkAvatar.TrimStart('/'));
						if (System.IO.File.Exists(oldFilePath))
						{
							System.IO.File.Delete(oldFilePath); // Xóa ảnh cũ
						}
					}

					// Tạo tên file ảnh mới duy nhất
					var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(LinkAvatarFile.FileName);
					var filePath = Path.Combine(uploadsFolder, uniqueFileName);

					// Lưu ảnh mới
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await LinkAvatarFile.CopyToAsync(fileStream);
					}

					// Cập nhật đường dẫn ảnh mới
					existingMember.LinkAvatar = "/image/" + uniqueFileName;
				}

				_context.Update(existingMember);
				await _context.SaveChangesAsync();


				TempData["Success"] = "Cập nhật thông tin cá nhân thành công!";
				return RedirectToAction(nameof(Index)); // Chuyển hướng về trang thông tin cá nhân
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine("Lỗi khi cập nhật: " + ex.Message);
				return View(member);
			}
		}
		public IActionResult ChangePassword()
		{
			return View();
		}
		// POST: RegTour/ChangePassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
		{
			int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;
			var member = await _context.Members.FindAsync(idMember);

			if (member == null)
			{
				TempData["Error"] = "Không tìm thấy người dùng!";
				return RedirectToAction("Index");
			}

			var passwordHasher = new PasswordHasher<Member>();

			// Kiểm tra mật khẩu hiện tại
			bool isPasswordValid = false;

			try
			{
				// Thử xác minh mật khẩu với PasswordHasher
				var result = passwordHasher.VerifyHashedPassword(member, member.Password, currentPassword);
				if (result == PasswordVerificationResult.Success)
				{
					isPasswordValid = true;
				}
			}
			catch (FormatException)
			{
				// Nếu lỗi, kiểm tra mật khẩu như là văn bản thuần
				if (member.Password == currentPassword)
				{
					isPasswordValid = true;
				}
			}

			if (!isPasswordValid)
			{
				TempData["Error"] = "Mật khẩu hiện tại không đúng.";
				return RedirectToAction("ChangePassword");
			}

			// Kiểm tra mật khẩu mới và xác nhận mật khẩu
			if (newPassword != confirmPassword)
			{
				TempData["Error"] = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
				return RedirectToAction("ChangePassword");
			}

			// Cập nhật mật khẩu (luôn mã hóa mới)
			try
			{
				member.Password = passwordHasher.HashPassword(member, newPassword);
				_context.Update(member);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Mật khẩu đã được thay đổi thành công.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
				return RedirectToAction("ChangePassword");
			}
		}



		private bool MemberExists(int id)
		{
			return _context.Members.Any(e => e.IdMember == id);
		}
	}
}
