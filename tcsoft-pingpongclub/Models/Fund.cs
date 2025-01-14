using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Fund
{
    [Key]
    public int IdFund { get; set; }
    [Required(ErrorMessage = "Tên quỹ không được để trống.")]
    public string? FundName { get; set; }
    [Required(ErrorMessage = "Tổng quỹ không được để trống.")]
    public decimal Total { get; set; }

    public bool? Status { get; set; } = true;

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();
}
