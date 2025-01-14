using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Reason
{
    [Key]
    public int IdReason { get; set; }
    [Required(ErrorMessage = "Tên nhà tài trợ không được để trống.")]
    public string? ReasonName { get; set; }
    [Required(ErrorMessage = "Loại không được để trống.")]
    public bool? Type { get; set; }

    public bool? Status { get; set; } = false;
    [Required(ErrorMessage = "Phí định kỳ không được để trống.")]
    public bool? recurringFee { get; set; }

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();
}
