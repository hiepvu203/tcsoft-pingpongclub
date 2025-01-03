using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class Tournament
{
    [Key]
    public int IdTournament { get; set; }

    [Required(ErrorMessage = "Tên giải đấu là bắt buộc")]
    public string TournamentName { get; set; }

    [Required(ErrorMessage = "Loại hình giải đấu là bắt buộc")]
    public bool? Type { get; set; }

    [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc")]
    public DateTime? TimeStart { get; set; }

    [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc")]
    [DateRange(nameof(TimeStart), nameof(TimeEnd))]
    public DateTime? TimeEnd { get; set; }
    public string? UrlImage { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(2, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 1 người ")]

    public short? Amount { get; set; }
    [NotMapped]
    public short? ActualAmount { get; set; } 

    [Required(ErrorMessage = "Thông tin của giải đấu là bắt buộc")]
    public string? Infor { get; set; }

    [Required(ErrorMessage = "Xếp hạng bắt đầu là bắt buộc")]
    public int? RankStart { get; set; }
    [Required(ErrorMessage = "Xếp hạng kết thúc là bắt buộc")]
    [RankRange(nameof(RankStart), nameof(RankEnd))]
    public int? RankEnd { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? Status { get; set; }

    [NotMapped]
    public IFormFile? ImageUpload { get; set; }
    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<Groupstage> Groupstages { get; set; } = new List<Groupstage>();

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual Level? RankEndNavigation { get; set; }

    public virtual Level? RankStartNavigation { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
