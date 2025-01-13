using Microsoft.AspNetCore.Mvc.Rendering;

namespace tcsoft_pingpongclub.ViewModels
{
    public class StatisticsViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string FundName { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public decimal OpeningBalance { get; set; }  // Số dư đầu kỳ
        public decimal ClosingBalance { get; set; }  // Số dư cuối kỳ
        //public int Month { get; set; } 
        //public int Year { get; set; } 
        //public string FundName { get; set; } 
        //public decimal StartingBalance { get; set; } 
        //public decimal TotalIncome { get; set; }
        //public decimal TotalExpense { get; set; }
        //public decimal EndingBalance { get; set; } 
    }
}
