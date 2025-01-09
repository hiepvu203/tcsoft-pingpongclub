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
    public class AwardsController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public AwardsController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Awards
        public async Task<IActionResult> Index(int? Id)
        {
            if (Id == null)
            {
                return NotFound("IdTournament không được truyền vào.");
            }
            var awards = await _context.Awards.Include(a => a.IdPlayerNavigation).ThenInclude(p => p.IdMemberNavigation)
                .Include(a => a.IdTournamentNavigation)
                .Include(a => a.IdNavigation)
                .Where(g => g.IdTournament == Id)
                .ToListAsync();
            ViewBag.IdTournament = Id;
            return View(awards);
        }

        // GET: Awards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .Include(a => a.IdPlayerNavigation)
                .ThenInclude(p => p.IdMemberNavigation)
                .Include(a => a.IdTournamentNavigation)
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdAward == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // GET: Awards/Create
        public IActionResult CreateMultiple(int Id)
        {
            ViewData["IdPlayer"] = new SelectList(
                _context.Players
                    .Where(p => p.IdTournament == Id)
                    .Include(p => p.IdMemberNavigation)
                    .ToList(),
                "IdPlayer",
                "IdMemberNavigation.MemberName");
            ViewBag.IdTournament = Id;
            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id");

            return View(); 
        }

        // POST: Awards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultiple(List<Award> awards)
        {
            decimal totalMoney = awards.Sum(a => a.Money ?? 0); // Sử dụng toán tử null-coalescing (??) để tránh null

            // Lấy Amount từ ExpenseAndIncome
            var expenseAndIncome = _context.ExpenseAndIncomes.FirstOrDefault(e => e.Id == awards.First().Id);
            if (expenseAndIncome != null)
            {
                decimal amount = expenseAndIncome.Amount ?? 0;

                // Kiểm tra nếu tổng tiền của giải thưởng bằng Amount
                if (totalMoney != amount)
                {
                    // Hiển thị thông báo lỗi nếu tổng tiền không bằng Amount
                    ModelState.AddModelError(string.Empty, "Tổng tiền của 3 giải thưởng không bằng số tiền của giao dịch.");
                    ViewData["IdPlayer"] = new SelectList(
                _context.Players
                    .Where(p => p.IdTournament == awards.First().IdTournament)
                    .Include(p => p.IdMemberNavigation)
                    .ToList(),
                "IdPlayer",
                "IdMemberNavigation.MemberName",
                awards.First().IdPlayer);
                    ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id", awards.First().Id);
                    ViewBag.IdTournament = awards.First().IdTournament;
                    return View(awards); // Trả lại view với lỗi
                }
            }
            if (ModelState.IsValid)
            {
                // Add each award to the context
                foreach (var award in awards)
                {
                    _context.Add(award);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Id = awards.First().IdTournament });
            }

            ViewData["IdPlayer"] = new SelectList(
                _context.Players
                    .Where(p => p.IdTournament == awards.First().IdTournament)
                    .Include(p => p.IdMemberNavigation)
                    .ToList(),
                "IdPlayer",
                "IdMemberNavigation.MemberName",
                awards.First().IdPlayer);
            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id", awards.First().Id);
            ViewBag.IdTournament = awards.First().IdTournament;
            return View(awards);
        }



        // GET: Awards/Create
        public IActionResult Create(int Id)
        {
            ViewData["IdPlayer"] = new SelectList(
             _context.Players
                 .Where(p => p.IdTournament ==Id)
                 .Include(p => p.IdMemberNavigation)
                 .ToList(),  // Chuyển về List để sử dụng với SelectList
             "IdPlayer",
             "IdMemberNavigation.MemberName");
            ViewBag.IdTournament = Id;
            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes
                .Include(p => p.IdTournamentNavigation).Where(p => p.IdTournament==Id)
                .Select(f => new { Id = f.Id, Display = f.IdTournamentNavigation.TournamentName + ": " + (f.IdTournamentNavigation.TimeStart.HasValue ? f.IdTournamentNavigation.TimeStart.Value.ToString("dd/MM/yyyy") : "Không xác định") + " - Mã Hóa Đơn: " + (f.Id)}), "Id", "Display");
            return View();
        }


        // POST: Awards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAward,IdTournament,IOrder,Money,Score,IdPlayer,Id,Status")] Award award)
        {
            if (ModelState.IsValid)
            {
                _context.Add(award);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { Id = award.IdTournament });
            }
            ViewData["IdPlayer"] = new SelectList(
             _context.Players
                 .Where(p => p.IdTournament == award.IdTournament)
                 .Include(p => p.IdMemberNavigation)
                 .ToList(),  // Chuyển về List để sử dụng với SelectList
             "IdPlayer",
             "IdMemberNavigation.MemberName",  // Hiển thị tên của Member từ IdMemberNavigation
             award.IdPlayer);
            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id", award.Id);
            ViewBag.IdTournament = award.IdTournament;
            return View(award);
        }

        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }
            ViewData["IdPlayer"] = new SelectList(
             _context.Players
                 .Where(p => p.IdTournament == award.IdTournament)
                 .Include(p => p.IdMemberNavigation)
                 .ToList(),  // Chuyển về List để sử dụng với SelectList
             "IdPlayer",
             "IdMemberNavigation.MemberName",  // Hiển thị tên của Member từ IdMemberNavigation
             award.IdPlayer);

            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id", award.Id);
            ViewBag.IdTournament = award.IdTournament;
            return View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAward,IdTournament,IOrder,Money,Score,Id,IdPlayer,Status")] Award award)
        {
         
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(award);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(award.IdAward))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { Id = award.IdTournament });
            }

            // Gán lại danh sách IdPlayer và Id (trong trường hợp ModelState không hợp lệ)
            ViewData["IdPlayer"] = new SelectList(
                _context.Players
                    .Where(p => p.IdTournament == award.IdTournament)
                    .Include(p => p.IdMemberNavigation)
                    .ToList(),  // Chuyển về List để sử dụng với SelectList
                "IdPlayer",
                "IdMemberNavigation.MemberName",  // Hiển thị tên của Member từ IdMemberNavigation
                award.IdPlayer);
            ViewData["Id"] = new SelectList(_context.ExpenseAndIncomes.Where(p => p.Type == true), "Id", "Id", award.Id);
            ViewBag.IdTournament = award.IdTournament;
            return View(award);
        }


        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .Include(a => a.IdPlayerNavigation)
                .ThenInclude(p => p.IdMemberNavigation)
                .Include(a => a.IdTournamentNavigation)
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdAward == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var award = await _context.Awards.FindAsync(id);
            var IdTournament = award.IdTournament;
            if (award != null)
            {
                _context.Awards.Remove(award);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { Id = IdTournament });
        }

        private bool AwardExists(int id)
        {
            return _context.Awards.Any(e => e.IdAward == id);
        }
    }
}
