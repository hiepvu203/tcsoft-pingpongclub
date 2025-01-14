using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace tcsoft_pingpongclub.Controllers
{
   
    public class TournamentsController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public TournamentsController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Tournaments
        public async Task<IActionResult> Index()
        {        
            var tournamentsWithActualAmount = await _context.Tournaments
                .Include(t => t.RankEndNavigation)
                .Include(t => t.RankStartNavigation)
                .Select(t => new Tournament
                {
                    IdTournament = t.IdTournament,
                    TournamentName = t.TournamentName,
                    UrlImage = t.UrlImage,
                    TimeStart = t.TimeStart,
                    Type = t.Type,
                    TimeEnd = t.TimeEnd,
                    Infor = t.Infor,
                    Amount = t.Amount,
                    RankStartNavigation = t.RankStartNavigation,
                    RankEndNavigation = t.RankEndNavigation,
                    Status = t.Status,
                    ActualAmount = (short)_context.Players.Count(p => p.IdTournament == t.IdTournament)
                })
                .OrderByDescending(t => t.IdTournament)                
                .ToListAsync();
            return View(tournamentsWithActualAmount);
        }


        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.RankEndNavigation)
                .Include(t => t.RankStartNavigation)
                .Include(t => t.Groupstages)
                .FirstOrDefaultAsync(m => m.IdTournament == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }


        // GET: Tournaments/Create
        public IActionResult Create()
        {
            ViewData["RankEnd"] = new SelectList(_context.Levels, "IdLevel", "LevelName");
            ViewData["RankStart"] = new SelectList(_context.Levels, "IdLevel", "LevelName");
            ViewBag.TypeList = new SelectList(new[]
            {
                new { Value = true, Text = "Đấu Cúp" },
                new { Value = false, Text = "Đấu Vòng" }
            }, "Value", "Text");
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = true, Text = "Đang diễn ra" },
                new { Value = false, Text = "Ẩn" }
            }, "Value", "Text");
            return View();
        }

        // POST: Tournaments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tournament tournament)
        {


            if (ModelState.IsValid && tournament.ImageUpload != null)
            {
                // Lấy tên tệp và thêm định danh duy nhất nếu cần
                var fileName = Path.GetFileNameWithoutExtension(tournament.ImageUpload.FileName);
                var extension = Path.GetExtension(tournament.ImageUpload.FileName);
                fileName = $"{fileName}_{Guid.NewGuid()}{extension}"; // Thêm định danh duy nhất
                var filePath = Path.Combine("wwwroot/image", fileName);

                // Đảm bảo thư mục tồn tại
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Lưu tệp vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await tournament.ImageUpload.CopyToAsync(stream);
                }

                // Lưu URL của tệp vào model
                tournament.UrlImage = "/image/" + fileName;
                // Thêm dữ liệu vào cơ sở dữ liệu
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(nameof(tournament.ImageUpload), "Vui lòng chọn hình ảnh.");
                ViewData["RankEnd"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankEnd);
                ViewData["RankStart"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankStart);
                ViewBag.TypeList = new SelectList(new[] { new { Value = true, Text = "Đấu Cúp" }, new { Value = false, Text = "Đấu Vòng" } }, "Value", "Text", tournament.Type);
                ViewBag.StatusList = new SelectList(new[] { new { Value = true, Text = "Đang diễn ra" }, new { Value = false, Text = "Ẩn" } }, "Value", "Text", tournament.Status);
            }
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            ViewData["RankEnd"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankEnd);
            ViewData["RankStart"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankStart);
            ViewBag.TypeList = new SelectList(new[]
            {
                new { Value = true, Text = "Đấu Cúp" },
                new { Value = false, Text = "Đấu Vòng" }
            }, "Value", "Text", tournament.Type);
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = true, Text = "Đang Diễn Ra" },
                new { Value = false, Text = "Ẩn" }
            }, "Value", "Text", tournament.Status);
            ViewData["UrlImage"] = tournament.UrlImage;
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTournament,TournamentName,Type,TimeStart,TimeEnd,UrlImage,Amount,RankStart,RankEnd,Infor,Status,ImageUpload")] Tournament tournament)
        {
            if (id != tournament.IdTournament)
            {
                return RedirectToAction(nameof(Index));
            }

            // Lấy giải đấu hiện tại trong cơ sở dữ liệu
            var existingTournament = await _context.Tournaments
                .FirstOrDefaultAsync(t => t.IdTournament == id);

            if (existingTournament == null)
            {
                return NotFound(); // Không tìm thấy giải đấu
            }
            if (ModelState.IsValid)
            {
                try
                {


                    // Kiểm tra và xử lý hình ảnh nếu có
                    if (tournament.ImageUpload != null)
                    {
                        // Lấy đường dẫn của ảnh cũ
                        var oldUrlImage = existingTournament.UrlImage;
                        if (!string.IsNullOrEmpty(oldUrlImage))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldUrlImage.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath); // Xóa tệp ảnh cũ
                            }
                        }

                        // Tạo tên tệp duy nhất
                        var fileName = $"{Path.GetFileNameWithoutExtension(tournament.ImageUpload.FileName)}_{Guid.NewGuid()}{Path.GetExtension(tournament.ImageUpload.FileName)}";
                        var filePath = Path.Combine("wwwroot/image", fileName);

                        // Đảm bảo thư mục tồn tại
                        var directory = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Lưu tệp vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await tournament.ImageUpload.CopyToAsync(stream);
                        }

                        // Cập nhật URL của tệp vào model
                        tournament.UrlImage = "/image/" + fileName;
                    }
                    else
                    {
                        // Giữ nguyên UrlImage nếu không thay đổi ảnh
                        tournament.UrlImage = existingTournament.UrlImage;
                    }

                    // Cập nhật thông tin giải đấu vào cơ sở dữ liệu
                    _context.Entry(existingTournament).CurrentValues.SetValues(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.IdTournament))
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

            ViewData["RankEnd"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankEnd);
            ViewData["RankStart"] = new SelectList(_context.Levels, "IdLevel", "LevelName", tournament.RankStart);
            ViewBag.TypeList = new SelectList(new[]
            {
                new { Value = true, Text = "Đấu Cúp" },
                new { Value = false, Text = "Đấu Vòng" }
            }, "Value", "Text", tournament.Type);
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = true, Text = "Đang Diễn Ra" },
                new { Value = false, Text = "Ẩn" }
            }, "Value", "Text", tournament.Status);
            ViewData["UrlImage"] = existingTournament.UrlImage;
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.RankEndNavigation)
                .Include(t => t.RankStartNavigation)
                .FirstOrDefaultAsync(m => m.IdTournament == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            // Kiểm tra xem UrlImage có chứa giá trị hợp lệ không
            if (!string.IsNullOrEmpty(tournament.UrlImage))
            {
                // Lấy đường dẫn đầy đủ của file cũ
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", tournament.UrlImage.TrimStart('/'));

                // Kiểm tra xem file có tồn tại không và xóa
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath); // Xóa ảnh cũ
                }

            }
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.IdTournament == id);
        }
    }
}
