using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Award
{
    [Key]
    public int IdAward { get; set; }

    public int? IdTournament { get; set; }

    [Required(ErrorMessage = "Giải thưởng không được để trống.")]
    public short? IOrder { get; set; }
    [Required(ErrorMessage = "Không để trống tiền thưởng")]

    [Range(0, double.MaxValue, ErrorMessage = "Tiền phải là giá trị không âm.")]
    [Column(TypeName = "decimal(18, 2)")] // Xác định độ chính xác của trường Money
    public decimal? Money { get; set; }
    [Required(ErrorMessage = "Điểm không được để trống.")]

    [Range(0, short.MaxValue, ErrorMessage = "Score phải là giá trị không âm.")]
    public short? Score { get; set; }

    public int? IdPlayer { get; set; }

    [Required(ErrorMessage = "Id hóa đơn chi không được để trống.")]
    public int? Id { get; set; }
    [Required(ErrorMessage = "Trạng thái không được để trống.")]
    public bool? Status { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }

    public virtual Player? IdPlayerNavigation { get; set; }

    public virtual ExpenseAndIncome? IdNavigation { get; set; }
}