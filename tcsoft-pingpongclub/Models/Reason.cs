using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Reason
{
    public int IdReason { get; set; }

    public string? ReasonName { get; set; }

    public bool? Type { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();
}
