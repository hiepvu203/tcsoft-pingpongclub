using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Fund
{
    public int IdFund { get; set; }

    public string? FundName { get; set; }

    public decimal? Total { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();
}
