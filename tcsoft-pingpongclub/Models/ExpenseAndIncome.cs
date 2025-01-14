using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class ExpenseAndIncome
{
	[Key]
	public int Id { get; set; }
	[ForeignKey("Fund")]
	public int? IdFund { get; set; }
	[ForeignKey("Member")]
	public int? IdParty { get; set; }
	[ForeignKey("Member")]
	public int? IdAccountant { get; set; }
	[ForeignKey("Tournament")]
	public int? IdTournament { get; set; }
	[ForeignKey("Sponor")]
	public int? IdSponorDetail { get; set; }

	public bool? IsDone { get; set; } = false;
	[Required(ErrorMessage = "Loại không được để trống.")]
	public bool? Type { get; set; }
	[Required(ErrorMessage = "Lý do không được để trống.")]
	[ForeignKey("Reason")]
	public int? IdReason { get; set; }

	public short? DaysOverdue { get; set; } = 0;

	public bool? Status { get; set; } = false;
	[Required(ErrorMessage = "Thời gian không được để trống.")]
	[DataType(DataType.Date, ErrorMessage = "Ngày phải đúng định dạng ngày.")]
	public DateTime? CreatedDate { get; set; }

	public virtual Fund? IdFundNavigation { get; set; }

	public virtual Member? IdPartyNavigation { get; set; }

	public virtual Reason? IdReasonNavigation { get; set; }

	public virtual Tournament? IdTournamentNavigation { get; set; }

	public virtual Sponor? IdSponorDetailNavigation { get; set; }

	[NotMapped]
	public string? AccountantName { get; set; }

	[NotMapped]
	public string? SponorName { get; set; }

	[Required(ErrorMessage = "Số tiền không được để trống.")]
	[Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0.")]
	public decimal Amount { get; set; }
	
	 public virtual ICollection<Award> Awards { get; set; } = new List<Award>();
}