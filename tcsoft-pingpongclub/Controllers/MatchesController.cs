using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using tcsoft_pingpongclub.Hubs;
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;
        private readonly IHubContext<SetRatio> _hubContext;

        public MatchesController(ThuctapKtktcn2024Context context, IHubContext<SetRatio> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IEnumerable<object> getRank(int id)
         {
         var RankTour = _context.Players.Where(p => p.IdTournament == id)
                                    .Select(player => new
                                    {
                                        Id = player.IdPlayer,
                                        UrlAvatar = _context.Members.Where(m => m.IdMember == player.IdMember).Select(m => m.LinkAvatar).FirstOrDefault() ?? String.Empty,
                                        NamePlayer = _context.Members.Where(m => m.IdMember == player.IdMember).Select(m => m.MemberName).FirstOrDefault() ?? String.Empty,
                                        CountMatch = _context.Matches.Where(match => match.IdMemberWin == player.IdPlayer).Count()
                                    }).OrderByDescending(m => m.CountMatch);
            return RankTour;
        }

        // GET: Matches
        [Route("Matches/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var RankTour = getRank(id);
            string NameTour = await _context.Tournaments.Where(m => m.IdTournament == id).Select(m => m.TournamentName).FirstOrDefaultAsync();
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
         urlPlayer1 = _context.Players
             .Where(m => m.IdPlayer == p.IdMemberOne)
             .Select(m => _context.Members
                 .Where(k => k.IdMember == m.IdMember)
                 .Select(k => k.LinkAvatar)
                 .FirstOrDefault())
             .FirstOrDefault()??string.Empty,
         urlPlayer2 = _context.Players
             .Where(m => m.IdPlayer == p.IdMemberTwo)
             .Select(m => _context.Members
                 .Where(k => k.IdMember == m.IdMember)
                 .Select(k => k.LinkAvatar)
                 .FirstOrDefault())
             .FirstOrDefault()??string.Empty, 

         PlayerName2 = _context.Players
             .Where(m => m.IdPlayer == p.IdMemberTwo)
             .Select(m => _context.Members
                 .Where(k => k.IdMember == m.IdMember)
                 .Select(k => k.MemberName)
                 .FirstOrDefault())
             .FirstOrDefault() ?? string.Empty,
         TimeStart =(DateTime) p.TimeStart,
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
            ViewBag.NameTour = NameTour;
            ViewBag.RankTour = RankTour;
            return View(await Match.ToListAsync());
        }
        public IEnumerable<object> getMatch(int id ,DateTime startDate,DateTime endDate)
        {
            var groupedMatches = _context.Matches
    .Where(m => m.IdTournament == id && 
               (m.TimeStart.Value.Date >= startDate.Date && m.TimeStart.Value.Date <= endDate.Date))
    .GroupBy(m => m.TimeStart.Value.Date)
    .Select(group => new
    {
        Date = group.Key,
        Matches = group.Select(p => new 
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
                .FirstOrDefault() ?? string.Empty,
            urlPlayer1 = _context.Players
                .Where(m => m.IdPlayer == p.IdMemberOne)
                .Select(m => _context.Members
                    .Where(k => k.IdMember == m.IdMember)
                    .Select(k => k.LinkAvatar)
                    .FirstOrDefault())
                .FirstOrDefault() ?? string.Empty,
            urlPlayer2 = _context.Players
                .Where(m => m.IdPlayer == p.IdMemberTwo)
                .Select(m => _context.Members
                    .Where(k => k.IdMember == m.IdMember)
                    .Select(k => k.LinkAvatar)
                    .FirstOrDefault())
                .FirstOrDefault() ?? string.Empty,

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
        }).ToList()
    }).ToList();
            return groupedMatches;
        }
        [Route("Matches/MatchClient/{id}")]
        public async Task<IActionResult> MatchClient(int id,DateTime TimeSearch)
        { 
            DateTime timeEnd = new DateTime(2100, 1, 1);
                DateTime timeStart = new DateTime(2000, 1, 1);
                DateTime timeNow = DateTime.Now;
                DateTime yesterday = DateTime.Now.AddDays(-1);
                DateTime tomorrow = DateTime.Now.AddDays(1);
            var Tour = await _context.Tournaments.Where(m => m.IdTournament == id).FirstOrDefaultAsync();      
            var RankTour = getRank(id);
            if (TimeSearch == default(DateTime))
            {
                ViewBag.futureMatches = getMatch(id, tomorrow, timeEnd);
                ViewBag.resultMatches = getMatch(id, timeStart, yesterday); 
                ViewBag.todayMatches = getMatch(id, timeNow, timeNow);
                ViewBag.ResultSearch = null;

            }
            else
            {
                ViewBag.ResultSearch = getMatch(id, TimeSearch, TimeSearch);
            }
            ViewBag.TimeSearch = TimeSearch;
              ViewBag.Id = id;
                ViewBag.Tour = Tour;
            ViewBag.RankTour = RankTour;

           
            return View();
        }
        [Route("Matches/MatchCenter")]
        public async Task<IActionResult> MatchCenter(int id)
        {
            DateTime timeNow = DateTime.Now;
           
            var Match = await _context.Matches.Where(m => m.IdMatch == id)
                                .Select(m => new
                                {
                                    IdTour = _context.Tournaments.Where(t => t.IdTournament == m.IdTournament).Select(t => t.IdTournament).FirstOrDefault(),
                                    NameTour = _context.Tournaments.Where(t => t.IdTournament == m.IdTournament).Select(t => t.TournamentName).FirstOrDefault(),
                                    PlayerName1 = _context.Players
                                         .Where(p => p.IdPlayer == m.IdMemberOne)
                                         .Select(p => _context.Members
                                             .Where(mem => mem.IdMember == p.IdMember)
                                             .Select(mem => mem.MemberName)
                                             .FirstOrDefault())
                                         .FirstOrDefault() ?? string.Empty,
                                     urlPlayer1 = _context.Players
                                         .Where(p => p.IdPlayer == m.IdMemberOne)
                                         .Select(p => _context.Members
                                             .Where(mem => mem.IdMember == p.IdMember)
                                             .Select(mem => mem.LinkAvatar)
                                             .FirstOrDefault())
                                         .FirstOrDefault() ?? string.Empty,
                                     urlPlayer2 = _context.Players
                                         .Where(p => p.IdPlayer == m.IdMemberTwo)
                                         .Select(p => _context.Members
                                             .Where(mem => mem.IdMember == p.IdMember)
                                             .Select(mem => mem.LinkAvatar)
                                             .FirstOrDefault())
                                         .FirstOrDefault() ?? string.Empty,
                                    PlayerName2 = _context.Players
                                         .Where(p => p.IdPlayer == m.IdMemberTwo)
                                         .Select(p => _context.Members
                                             .Where(mem => mem.IdMember == p.IdMember)
                                             .Select(mem => mem.MemberName)
                                             .FirstOrDefault())
                                         .FirstOrDefault() ?? string.Empty,
                                    TimeStart = m.TimeStart,
                                    Points1 = _context.Sets
                                                .Where(set => set.IdMatch == m.IdMatch && set.IdWinner ==m.IdMemberOne).Count(),
                                    Points2 = _context.Sets
                                .Where(set => set.IdMatch == m.IdMatch && set.IdWinner == m.IdMemberTwo)
                                .Count(),
                                ListSet = _context.Sets.Where(set => set.IdMatch == m.IdMatch).ToList()
                                }).FirstOrDefaultAsync();
            ViewBag.Match = Match;
            ViewBag.todayMatches = getMatch(Match.IdTour, timeNow, timeNow);
            return View();
        }
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
            var Player1 = await _context.Members.FindAsync(idPlayer1.IdMember);
            var Player2 = await _context.Members.FindAsync(idPlayer2.IdMember);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["IdGroupstage"] = new SelectList(_context.Groupstages, "IdGroupstage", "IdGroupstage", match.IdGroupstage);
           // ViewData["namePlayer1"] = namePlayer1.MemberName;
            ViewBag.playerName1 = Player1.MemberName;
            ViewBag.playerName2 = Player2.MemberName;
             ViewBag.urlPlayer1 = Player1.LinkAvatar;
            ViewBag.urlPlayer2 = Player2.LinkAvatar;
            ViewBag.Id = match.IdTournament;
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
            var Player1 = await _context.Members.FindAsync(idPlayer1.IdMember);
            var Player2 = await _context.Members.FindAsync(idPlayer2.IdMember);
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
           // ViewData["namePlayer1"] = namePlayer1.MemberName;
            ViewBag.playerName1 = Player1.MemberName;
            ViewBag.playerName2 = Player2.MemberName;
            ViewBag.urlPlayer1 = Player1.LinkAvatar;
            ViewBag.urlPlayer2 = Player2.LinkAvatar;
            ViewBag.idMatch = match.IdMatch;
            ViewBag.nameTour = nameTour.TournamentName;
            ViewBag.MatchTime = match.TimeStart;
            ViewBag.Id = match.IdTournament;
            return View(match);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSet( int id,  int point1,  int point2, int point3, int point4,  int point5,  int point6)
        {
          

                var match = await _context.Matches.FirstOrDefaultAsync(m => m.IdMatch == id);
                var mem1 = _context.Members
                                    .Where(p => p.IdMember == _context.Players
                                    .Where(p => p.IdPlayer == match.IdMemberOne).Select(p => p.IdMember).FirstOrDefault())
                                    .FirstOrDefault();
                var mem2 = _context.Members
                                    .Where(p => p.IdMember == _context.Players.Where(p => p.IdPlayer == match.IdMemberTwo).Select(p => p.IdMember).FirstOrDefault()).FirstOrDefault();

                int checkScore = Math.Abs((int)mem1.Score - (int)mem2.Score);
                var ScoreCal = _context.ScoreCals.Where(m => m.PtsMin <= checkScore && m.PtsMax >= checkScore).FirstOrDefault();
                var LevelMem1 = _context.Levels.Where(l => l.IdLevel == mem1.IdLevel).Select(l => l.LevelName).FirstOrDefault();
                var LevelMem2 = _context.Levels.Where(l => l.IdLevel == mem2.IdLevel).Select(l => l.LevelName).FirstOrDefault();
                var sets = await _context.Sets.Where(set => set.IdMatch == id).ToListAsync();
                if (!sets.Any())
                {
                    return NotFound();
                }

                if (match == null)
                {
                    return NotFound();
                }
                var ratio1 = $"{point1} - {point2}";
                var ratio2 = $"{point3} - {point4}";
                var ratio3 = $"{point5} - {point6}";
                int ratioMatch1 = 0;
                int ratioMatch2 = 0;


                var set1 = sets.FirstOrDefault(s => s.SetName == "1");
                if (set1 != null)
                {
                    set1.Ratio = ratio1;
                    if (point1 > point2 && point1 >= 21)
                    {
                        set1.IdWinner = (int)match.IdMemberOne;
                        ratioMatch1++;
                    }
                    else if (point1 < point2 && point2 >= 21)
                    {
                        set1.IdWinner = (int)match.IdMemberTwo;
                        ratioMatch2++;
                    }

                    _context.Entry(set1).State = EntityState.Modified;
                }

                var set2 = sets.FirstOrDefault(s => s.SetName == "2");
                if (set2 != null)
                {
                    set2.Ratio = ratio2;
                    if (point3 > point4 && point3 >= 21)
                    {
                        set2.IdWinner = (int)match.IdMemberOne;
                        ratioMatch1++;
                    }
                    else if (point3 < point4 && point4 >= 21)
                    {
                        set2.IdWinner = (int)match.IdMemberTwo;
                        ratioMatch2++;
                    }
                    _context.Entry(set2).State = EntityState.Modified;
                }

                var set3 = sets.FirstOrDefault(s => s.SetName == "3");
                if (set3 != null)
                {
                    set3.Ratio = ratio3;
                    if (point5 > point6 && point5 >= 21)
                    {
                        set3.IdWinner = (int)match.IdMemberOne;
                        ratioMatch1++;
                    }
                    else if (point5 < point6 && point6 >= 21)
                    {
                        set3.IdWinner = (int)match.IdMemberTwo;
                        ratioMatch2++;
                    }
                    _context.Entry(set3).State = EntityState.Modified;
                }
                if (ratioMatch1 > ratioMatch2)
                {
                    match.IdMemberWin = match.IdMemberOne;
                    if (String.Compare(LevelMem1, LevelMem2) >= 0)
                    {
                        mem1.Score += ScoreCal.PtsSameRankWin;
                        mem2.Score -= ScoreCal.PtsHighRankDef;
                    }
                    else
                    {
                        mem1.Score += ScoreCal.PtsHighRankWin;
                        mem2.Score -= ScoreCal.PtsSameRankDef;
                    }
                }
                else
                {
                    match.IdMemberWin = match.IdMemberTwo;
                    if (String.Compare(LevelMem2, LevelMem1) >= 0)
                    {
                        mem2.Score += ScoreCal.PtsSameRankWin;
                        mem1.Score -= ScoreCal.PtsHighRankDef;
                    }
                    else
                    {
                        mem2.Score += ScoreCal.PtsHighRankWin;
                        mem1.Score -= ScoreCal.PtsSameRankDef;
                    }
                }
                _context.Entry(match).State = EntityState.Modified;
                _context.Entry(mem1).State = EntityState.Modified;
                _context.Entry(mem2).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("GetRatio", id, ratio1, ratio2, ratio3);
                var idTournament = _context.Matches
                    .Where(m => m.IdMatch == id)
                    .Select(m => m.IdTournament)
                    .FirstOrDefault();
                return RedirectToAction("EditSet", "Matches", new { id = id });  
           //  return RedirectToAction("Index", "Matches", new { id = idTournament });
        }

        // GET: Matches/Delete/5
   
        // POST: Matches/Delete/5
       
        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.IdMatch == id);
        }
    }
}
