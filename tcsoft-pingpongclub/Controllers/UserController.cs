using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class UserController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public UserController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;

            if (idMember <= 0)
            {
                TempData["Error"] = "Người dùng không hợp lệ!";
                return View(); // Trả về view với thông báo lỗi
            }

            var member = await _context.Members
                .Include(m => m.IdLevelNavigation)
                .Include(m => m.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdMember == idMember);

            if (member == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng!";
                return View(); // Trả về view với thông báo lỗi
            }

            return View(member);
        }

        public IActionResult Information(int id)
        {
            // Lấy giải đấu hiện tại
            var tournament = _context.Tournaments
                .Include(t => t.RankStartNavigation)
                .Include(t => t.RankEndNavigation)
                .FirstOrDefault(t => t.IdTournament == id);

            if (tournament == null)
            {
                return NotFound(); // Nếu không tìm thấy, trả về 404
            }

            // Lấy danh sách các giải đấu khác trong cùng hạng
            var relatedTournaments = _context.Tournaments
                .Include(t => t.RankStartNavigation)
                .Include(t => t.RankEndNavigation)
                .Where(t => t.RankStart == tournament.RankStart && t.RankEnd == tournament.RankEnd && t.IdTournament != id)
                .Take(3)
                .ToList();

            // Gửi dữ liệu sang view
            ViewBag.RelatedTournaments = relatedTournaments;

            return View(tournament);
        }
        // GET: User/Edit
        public async Task<IActionResult> Edit()
        {
            // Lấy IdMember từ session
            int idMember = HttpContext.Session.GetInt32("IdMember") ?? 0;

            // Tìm thành viên trong cơ sở dữ liệu
            var member = await _context.Members.FindAsync(idMember);
            if (member == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            // Lấy danh sách hình ảnh cho lựa chọn avatar
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
            if (Directory.Exists(imagesPath))
            {
                var files = Directory.GetFiles(imagesPath).Select(Path.GetFileName).ToList();
                ViewBag.ImageFiles = files;
            }
            else
            {
                ViewBag.ImageFiles = new List<string>(); // Đảm bảo ViewBag không null
            }

            // Điền danh sách dropdown cho vai trò và cấp độ
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", member.IdLevel);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", member.IdRole);

            return View(member);
        }

        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdMember,MemberName,Address,Phone,Emaill,Gender,IdLevel,Status,LinkAvatar,Username,Password,IdRole")] Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Profile updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.IdMember))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Re-populate dropdown lists and images in case of errors
            ViewData["IdLevel"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", member.IdLevel);
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "IdRole", member.IdRole);
            return View(member);
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.IdMember == id);
        }
    }
}
