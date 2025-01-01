using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public MatchesController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Matches
        [Route("Matches/{id}")]
        public async Task<IActionResult> Index(int id)
        {
      
            var Match = _context.Matches.Where(m => m.IdTournament == id)
     .Select(p => new MatchAndPlayer
     {
         IdMatch = p.IdMatch,
         IdPlayer1 = (int)p.IdMemberOne,
         IdPlayer2 = (int)p.IdMemberTwo,
         IdTournament = (int)p.IdTournament,

         PlayerName1 = _context.Players
             .Where(m => m.IdPlayer == p.IdMemberOne)
             .Select(m => _context.Members
                 .Where(k => k.IdMember == m.IdMember)
                 .Select(k => k.MemberName)
                 .FirstOrDefault())
             .FirstOrDefault()??string.Empty, 
         PlayerName2 = _context.Players
             .Where(m => m.IdPlayer == p.IdMemberTwo)
             .Select(m => _context.Members
                 .Where(k => k.IdMember == m.IdMember)
                 .Select(k => k.MemberName)
                 .FirstOrDefault())
             .FirstOrDefault() ?? string.Empty,
         TimeStart = p.TimeStart,
         IdGroupstage = p.IdGroupstage,
         Points1 = _context.Sets
    .Where(set => set.IdMatch == p.IdMatch && set.IdWinner == p.IdMemberOne)
    .Count(),
         Points2 = _context.Sets
    .Where(set => set.IdMatch == p.IdMatch && set.IdWinner == p.IdMemberTwo)
    .Count(),
         ListSet = _context.Sets.Where(set => set.IdMatch == p.IdMatch).ToList()
     });

        if(!Match.Any())
          {
                var Player = _context.Players.Where(k => k.IdTournament == id).Select(m => m.IdPlayer);
                int CountPlay = Player.Count();
                var players  = Player.ToList();
                List<Match> matches = new List<Match>();
                for (int i = 0; i < CountPlay - 1; i++){
                    for(int j = i + 1; j < CountPlay; j++)
                    {
                 
                        var newMatch = new Match()
                        {
                            IdMemberOne = players[i],
                            IdMemberTwo = players[j],
                            IdTournament = id,
                            Status = true
                        };
                       matches.Add(newMatch);
                    }
                }
               _context.Matches.AddRange(matches);
                        await _context.SaveChangesAsync();
            }
            ViewBag.Id = id;
            return View(await Match.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.IdGroupstageNavigation)
                .Include(m => m.IdMemberOneNavigation)
                .Include(m => m.IdMemberTwoNavigation)
                .Include(m => m.IdMemberWinNavigation)
                .Include(m => m.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdMatch == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["IdGroupstage"] = new SelectList(_context.Groupstages, "IdGroupstage", "IdGroupstage");
            ViewData["IdMemberOne"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer");
            ViewData["IdMemberTwo"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer");
            ViewData["IdMemberWin"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer");
            ViewData["IdTournament"] = new SelectList(_context.Tournaments, "IdTournament", "IdTournament");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMatch,IdTournament,IdMemberOne,IdMemberTwo,TimeStart,IdGroupstage,IdMemberWin,Status")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGroupstage"] = new SelectList(_context.Groupstages, "IdGroupstage", "IdGroupstage", match.IdGroupstage);
            ViewData["IdMemberOne"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer", match.IdMemberOne);
            ViewData["IdMemberTwo"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer", match.IdMemberTwo);
            ViewData["IdMemberWin"] = new SelectList(_context.Players, "IdPlayer", "IdPlayer", match.IdMemberWin);
            ViewData["IdTournament"] = new SelectList(_context.Tournaments, "IdTournament", "IdTournament", match.IdTournament);
            return View(match);
        }


        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            var nameTour = await _context.Tournaments.FindAsync(match.IdTournament);
            var idPlayer1 = await _context.Players.FindAsync(match.IdMemberOne);
            var idPlayer2 = await _context.Players.FindAsync(match.IdMemberTwo);
            var namePlayer1 = await _context.Members.FindAsync(idPlayer1.IdMember);
            var namePlayer2 = await _context.Members.FindAsync(idPlayer2.IdMember);

            if (match == null)
            {
                return NotFound();
            }
            ViewData["IdGroupstage"] = new SelectList(_context.Groupstages, "IdGroupstage", "IdGroupstage", match.IdGroupstage);
           // ViewData["namePlayer1"] = namePlayer1.MemberName;
            ViewBag.playerName1 = namePlayer1.MemberName;
            ViewBag.playerName2 = namePlayer2.MemberName;
            ViewBag.nameTour = nameTour.TournamentName;

            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime timeStart)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            // Cập nhật giá trị TimeStart
            match.TimeStart = timeStart;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.IdMatch))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = match.IdTournament });
            }

            return View(match);
        }
         public async Task<IActionResult> EditSet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var match = await _context.Matches.FindAsync(id);
            var nameTour = await _context.Tournaments.FindAsync(match.IdTournament);
            var idPlayer1 = await _context.Players.FindAsync(match.IdMemberOne);
            var idPlayer2 = await _context.Players.FindAsync(match.IdMemberTwo);
            var namePlayer1 = await _context.Members.FindAsync(idPlayer1.IdMember);
            var namePlayer2 = await _context.Members.FindAsync(idPlayer2.IdMember);
            var sets = await _context.Sets.Where(set => set.IdMatch == match.IdMatch).ToListAsync();
            List<String> SetPoints = new List<string>();
            if(match.TimeStart == null)
            {
                return RedirectToAction("Edit", "Matches", new { id = id });
            }
            ViewBag.Set1 = "";
            ViewBag.nameTour = "";
            if (!sets.Any())
            {
               for(int i = 1; i < 4; i++)
                {
                    var set = new Set()
                    {
                        IdMatch = (int)id,
                        Ratio = "0 - 0",
                        SetName = i.ToString(),
                        Status = true
                    };
                    _context.Add(set);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("EditSet", "Matches", new { id = id });
            }
            else
            {
                foreach(var set in sets)
                {
                    SetPoints.Add(set.Ratio);
                }
            }
            ViewBag.SetPoints = SetPoints;
            ViewData["IdGroupstage"] = new SelectList(_context.Groupstages, "IdGroupstage", "IdGroupstage", match.IdGroupstage);
           // ViewData["namePlayer1"] = namePlayer1.MemberName;
            ViewBag.playerName1 = namePlayer1.MemberName;
            ViewBag.playerName2 = namePlayer2.MemberName;
            ViewBag.nameTour = nameTour.TournamentName;
            ViewBag.MatchTime = match.TimeStart;

            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSet(int id, int point1, int point2, int point3, int point4, int point5, int point6)
        {
            // Lấy danh sách các set thuộc IdMatch
            var sets = await _context.Sets.Where(set => set.IdMatch == id).ToListAsync();
            if (!sets.Any())
            {
                return NotFound();
            }

            // Lấy trận đấu liên quan
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.IdMatch == id);
            if (match == null)
            {
                return NotFound();
            }

            // Tính toán tỷ số
            var ratio1 = $"{point1} - {point2}";
            var ratio2 = $"{point3} - {point4}";
            var ratio3 = $"{point5} - {point6}";

            int ratioMatch1 = 0; // Số lần thắng của MemberOne
            int ratioMatch2 = 0; // Số lần thắng của MemberTwo

            // Cập nhật Set 1
            var set1 = sets.FirstOrDefault(s => s.SetName == "1");
            if (set1 != null)
            {
                set1.Ratio = ratio1;
                if (point1 > point2)
                {
                    set1.IdWinner =(int) match.IdMemberOne;
                    ratioMatch1++;
                }
                else
                {
                    set1.IdWinner =(int) match.IdMemberTwo;
                    ratioMatch2++;
                }
                _context.Entry(set1).State = EntityState.Modified; // Đảm bảo thực thể được đánh dấu là đã thay đổi
            }

            // Cập nhật Set 2
            var set2 = sets.FirstOrDefault(s => s.SetName == "2");
            if (set2 != null)
            {
                set2.Ratio = ratio2;
                if (point3 > point4)
                {
                    set2.IdWinner =(int) match.IdMemberOne;
                    ratioMatch1++;
                }
                else
                {
                    set2.IdWinner =(int) match.IdMemberTwo;
                    ratioMatch2++;
                }
                _context.Entry(set2).State = EntityState.Modified; // Đảm bảo thực thể được đánh dấu là đã thay đổi
            }

            // Cập nhật Set 3
            var set3 = sets.FirstOrDefault(s => s.SetName == "3");
            if (set3 != null)
            {
                set3.Ratio = ratio3;
                if (point5 > point6)
                {
                    set3.IdWinner =(int) match.IdMemberOne;
                    ratioMatch1++;
                }
                else
                {
                    set3.IdWinner =(int) match.IdMemberTwo;
                    ratioMatch2++;
                }
                _context.Entry(set3).State = EntityState.Modified; // Đảm bảo thực thể được đánh dấu là đã thay đổi
            }

            // Cập nhật thông tin chiến thắng trong bảng Match
            if (ratioMatch1 > ratioMatch2)
            {
                match.IdMemberWin = match.IdMemberOne;
            }
            else
            {
                match.IdMemberWin = match.IdMemberTwo;
            }
            _context.Entry(match).State = EntityState.Modified; // Đảm bảo thực thể được đánh dấu là đã thay đổi

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng
            var idTournament = _context.Matches
                .Where(m => m.IdMatch == id)
                .Select(m => m.IdTournament)
                .FirstOrDefault();

            return RedirectToAction("Index", "Matches", new { id = idTournament });
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.IdGroupstageNavigation)
                .Include(m => m.IdMemberOneNavigation)
                .Include(m => m.IdMemberTwoNavigation)
                .Include(m => m.IdMemberWinNavigation)
                .Include(m => m.IdTournamentNavigation)
                .FirstOrDefaultAsync(m => m.IdMatch == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.IdMatch == id);
        }
    }
}
