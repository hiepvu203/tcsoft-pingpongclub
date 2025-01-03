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
    public class GroupstagesController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public GroupstagesController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }
        // GET: Groupstages
        public async Task<IActionResult> Index(int? IdTournament)
        {
            if (IdTournament == null)
            {
                return NotFound("IdTournament không được truyền vào.");
            }
            var groupStages = await _context.Groupstages
                .Include(g => g.IdTournamentNavigation)
                .Where(g => g.IdTournament == IdTournament)
                .ToListAsync();
            ViewBag.IdTournament = IdTournament;
            return View(groupStages);
        }
        // GET: Groupstages/Details/5
        public async Task<IActionResult> Details(int? id,int? IdTournament)
        {
            if (id == null || IdTournament==null)
            {
                return NotFound();
            }

            var groupstage = await _context.Groupstages
                .Include(g => g.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdGroupstage == id);
            if (groupstage == null)
            {
                return NotFound();
            }
            ViewBag.IdTournament = IdTournament;
            return View(groupstage);
        }


        // GET: Groupstages/Create
        public IActionResult Create(int Id)
        {
            ViewBag.IdTournament = Id;          
            return View();
        }

        public async Task<IActionResult> CreateTwoGroups(int Id)
        {
            var tournament = await _context.Tournaments.FindAsync(Id);
            if (tournament == null)
            {
                return NotFound();
            }

            int totalAmount = tournament.Amount ?? 0;

            // Tạo 2 bảng đấu với idorder = 1 (tổng số lượng = Số lượng giải đấu)
            int amountPerGroupOrder1 = totalAmount / 2;
            int remainderOrder1 = totalAmount % 2;

            for (int i = 0; i < 2; i++)
            {
                var group = new Groupstage
                {
                    IdTournament = Id,
                    NameGroup = $"Bảng {i + 1}",
                    Amount = amountPerGroupOrder1 + (i < remainderOrder1 ? 1 : 0),  // Phân bổ dư
                    IOrder = 1,
                    Status = true
                };
                _context.Groupstages.Add(group);
            }

            // Tạo 2 bảng đấu với idorder = 2 (2 người mỗi bảng)
            for (int i = 0; i < 2; i++)
            {
                var group = new Groupstage
                {
                    IdTournament = Id,
                    NameGroup = $"Bảng {i + 3}",
                    Amount = 2,
                    IOrder = 2,
                    Status = true
                };
                _context.Groupstages.Add(group);
            }

            // Tạo 1 bảng đấu với idorder = 3 (2 người)
            var group3 = new Groupstage
            {
                IdTournament = Id,
                NameGroup = "Bảng 5",
                Amount = 2,
                IOrder = 3,
                Status = true
            };
            _context.Groupstages.Add(group3);

            // Tạo 1 bảng đấu với idorder = 4 (2 người)
            var group4 = new Groupstage
            {
                IdTournament = Id,
                NameGroup = "Bảng 6",
                Amount = 2,
                IOrder = 4,
                Status = true
            };
            _context.Groupstages.Add(group4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { IdTournament = Id });
        }

        public async Task<IActionResult> CreateFourGroups(int Id)
        {
            var tournament = await _context.Tournaments.FindAsync(Id);
            if (tournament == null)
            {
                return NotFound();
            }

            int totalAmount = tournament.Amount ?? 0;

            // Tạo 4 bảng đấu với idorder = 1 (tổng số lượng = Số lượng giải đấu)
            int amountPerGroupOrder1 = totalAmount / 4;
            int remainderOrder1 = totalAmount % 4;

            for (int i = 0; i < 4; i++)
            {
                var group = new Groupstage
                {
                    IdTournament = Id,
                    NameGroup = $"Bảng {i + 1}",
                    Amount = amountPerGroupOrder1 + (i < remainderOrder1 ? 1 : 0), // Phân bổ dư
                    IOrder = 1,
                    Status = true
                };
                _context.Groupstages.Add(group);
            }

            // Tạo 4 bảng đấu với idorder = 2 (2 người mỗi bảng)
            for (int i = 0; i < 4; i++)
            {
                var group = new Groupstage
                {
                    IdTournament = Id,
                    NameGroup = $"Bảng {i + 5}",
                    Amount = 2,
                    IOrder = 2,
                    Status = true
                };
                _context.Groupstages.Add(group);
            }

            // Tạo 3 bảng đấu với idorder = 3 (2 người mỗi bảng)
            for (int i = 0; i < 3; i++)
            {
                var group = new Groupstage
                {
                    IdTournament = Id,
                    NameGroup = $"Bảng {i + 9}",
                    Amount = 2,
                    IOrder = 3,
                    Status = true
                };
                _context.Groupstages.Add(group);
            }

            // Tạo 1 bảng đấu với idorder = 4 (2 người)
            var group4 = new Groupstage
            {
                IdTournament = Id,
                NameGroup = "Bảng 12",
                Amount = 2,
                IOrder = 4,
                Status = true
            };
            _context.Groupstages.Add(group4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { IdTournament = Id });
        }


        // POST: Groupstages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Groupstage groupstage)
        {
            if (ModelState.IsValid)
            {
                _context.Groupstages.Add(groupstage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { IdTournament = groupstage.IdTournament });
            }
            ViewBag.IdTournament = groupstage.IdTournament;
            return View(groupstage);
        }


        // GET: Groupstages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupstage = await _context.Groupstages.FindAsync(id);
            if (groupstage == null)
            {
                return NotFound();
            }
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = true, Text = "Đang diễn ra" },
                new { Value = false, Text = "Ẩn" }
            }, "Value", "Text");
            ViewBag.IOrderList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Loại", Value = "1" },
                new SelectListItem { Text = "Tứ kết", Value = "2" },
                new SelectListItem { Text = "Bán kết", Value = "3" },
                new SelectListItem { Text = "Chung kết", Value = "4" }
            };
            // Truyền IdTournament vào ViewBag hoặc ViewModel
            ViewBag.IdTournament = groupstage.IdTournament;

            return View(groupstage);
        }


        // POST: Groupstages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGroupstage,NameGroup,Amount,IOrder,Status,IdTournament")] Groupstage groupstage)
        {
            if (id != groupstage.IdGroupstage)
            {
                return NotFound(); // Nếu id từ URL không khớp với IdGroupstage trong form thì báo lỗi
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupstage); // Cập nhật bảng đấu hiện tại
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupstageExists(groupstage.IdGroupstage))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index), new { IdTournament = groupstage.IdTournament });
            }
            ViewBag.IOrderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Loại", Selected = (groupstage.IOrder == 1) },
                new SelectListItem { Value = "2", Text = "Tứ kết", Selected = (groupstage.IOrder == 2) },
                new SelectListItem { Value = "3", Text = "Bán kết", Selected = (groupstage.IOrder == 3) },
                new SelectListItem { Value = "4", Text = "Chung kết", Selected = (groupstage.IOrder == 4) }

            };
            ViewBag.IdTournament = groupstage.IdTournament;
            return View(groupstage); // Nếu có lỗi, trả lại view để người dùng sửa lại
        }
        // GET: Groupstages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupstage = await _context.Groupstages
                .Include(g => g.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdGroupstage == id);
            if (groupstage == null)
            {
                return NotFound();
            }
            return View(groupstage);
        }

        // POST: Groupstages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupstage = await _context.Groupstages.FindAsync(id);
            var IdTournament = groupstage.IdTournament;
            if (groupstage != null)
            {
                _context.Groupstages.Remove(groupstage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { IdTournament = IdTournament });
        }
        public IActionResult DeleteAll(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var groupstages = _context.Groupstages.Where(g => g.IdTournament == Id).ToList();

            if (groupstages.Any())
            {
                _context.Groupstages.RemoveRange(groupstages);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { IdTournament = Id });
        }

        private bool GroupstageExists(int id)
        {
            return _context.Groupstages.Any(e => e.IdGroupstage == id);
        }
    }
}
