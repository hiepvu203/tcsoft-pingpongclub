using Microsoft.AspNetCore.Mvc.Rendering;

namespace tcsoft_pingpongclub.ViewModels
{
    public class StatisticsViewModel
    {
        public SelectList Items { get; set; }
        public string SelectedItem { get; set; }
        public List<TournamentStats> TournamentStats { get; set; }
        public List<MatchStats> MatchStats { get; set; }
        public List<IncomeStats> IncomeStats { get; set; }
        public List<ExpenseStats> ExpenseStats { get; set; }
    }

    public class TournamentStats
    {
        public int IdTournament { get; set; }
        public string TournamentName { get; set; }
        public int MatchCount { get; set; }
        public int PlayerCount { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }

    public class MatchStats
    {
        public int IdMatch { get; set; }
        public string TournamentName { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public string Winner { get; set; }
        public DateTime? TimeStart { get; set; }
        public bool? Status { get; set; }
    }

    public class IncomeStats
    {
        public string FundName { get; set; }
        public decimal TotalIncome { get; set; }
    }

    public class ExpenseStats
    {
        public string FundName { get; set; }
        public decimal TotalExpense { get; set; }
    }
}
