using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcsoft_pingpongclub.Models;
using tcsoft_pingpongclub.ViewModels;

namespace tcsoft_pingpongclub.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ThuctapKtktcn2024Context _context;

        public StatisticsController(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }

        // GET: Statistics/Index
        public async Task<IActionResult> Index(string selectedItem)
        {
            var viewModel = new StatisticsViewModel
            {
                Items = new SelectList(new[] { "Tournaments", "Matches", "IncomeAndExpense" })
            };

            if (!string.IsNullOrEmpty(selectedItem))
            {
                viewModel.SelectedItem = selectedItem;

                switch (selectedItem)
                {
                    case "Tournaments":
                        var tournaments = await _context.Tournaments
                            .Include(t => t.Matches)
                            .Include(t => t.Players)
                            .ToListAsync();

                        viewModel.TournamentStats = tournaments.Select(t => new TournamentStats
                        {
                            IdTournament = t.IdTournament,
                            TournamentName = t.TournamentName,
                            MatchCount = t.Matches.Count,
                            PlayerCount = t.Players.Count,
                            TimeStart = t.TimeStart,
                            TimeEnd = t.TimeEnd
                        }).ToList();
                        break;

                    case "Matches":
                        var matches = await _context.Matches
                            .Include(m => m.IdTournamentNavigation)
                            .Include(m => m.IdMemberOneNavigation)
                            .Include(m => m.IdMemberTwoNavigation)
                            .Include(m => m.IdMemberWinNavigation)
                            .ToListAsync();

                        viewModel.MatchStats = matches.Select(m => new MatchStats
                        {
                            IdMatch = m.IdMatch,
                            TournamentName = m.IdTournamentNavigation?.TournamentName,
                            PlayerOne = m.IdMemberOneNavigation?.IdMemberNavigation?.MemberName,
                            PlayerTwo = m.IdMemberTwoNavigation?.IdMemberNavigation?.MemberName,
                            Winner = m.IdMemberWinNavigation?.IdMemberNavigation?.MemberName,
                            TimeStart = m.TimeStart,
                            Status = m.Status
                        }).ToList();
                        break;

                    case "IncomeAndExpense":
                        var funds = await _context.Funds
                            .Include(f => f.ExpenseAndIncomes)
                            .ToListAsync();

                        viewModel.IncomeStats = funds
                            .Where(f => f.ExpenseAndIncomes.Any(e => e.Type == false))
                            .Select(f => new IncomeStats
                            {
                                FundName = f.FundName,
                                TotalIncome = f.ExpenseAndIncomes.Where(e => e.Type == false).Sum(e => e.IdFundNavigation.Total ?? 0)
                            }).ToList();

                        viewModel.ExpenseStats = funds
                            .Where(f => f.ExpenseAndIncomes.Any(e => e.Type == true))
                            .Select(f => new ExpenseStats
                            {
                                FundName = f.FundName,
                                TotalExpense = f.ExpenseAndIncomes.Where(e => e.Type == true).Sum(e => e.IdFundNavigation.Total ?? 0)
                            }).ToList();
                        break;
                }
            }

            return View(viewModel);
        }
    }
}