using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Sponor
{
    [Key]
    public int IdSponorTour { get; set; }
    [Required(ErrorMessage = "Tiền tài trợ không được để trống.")]
    public decimal Money { get; set; }

    public int? IdTournament { get; set; }

    public bool? Status { get; set; }

    public int? IdSponor { get; set; }

    public string? Other { get; set; }

    [Required(ErrorMessage = "Thời gian không được để trống.")]
    [DataType(DataType.Date, ErrorMessage = "Ngày phải đúng định dạng ngày.")]
    public DateTime? CreatedDate { get; set; }

    public virtual NhaTaiTro? IdSponorNavigation { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }
    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();
}
