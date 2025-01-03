using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Groupstage
{
    [Key]
    public int IdGroupstage { get; set; }

    [Required(ErrorMessage = "Tên bảng là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên bảng không được vượt quá 100 ký tự")]
    public string? NameGroup { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải là số dương và lớn hơn 0")]
    public int? Amount { get; set; }

    [Required(ErrorMessage = "Vòng đấu là bắt buộc")]
    public short? IOrder { get; set; }
    public int? IdTournament { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? Status { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
