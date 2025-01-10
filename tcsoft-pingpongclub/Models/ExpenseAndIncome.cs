using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class ExpenseAndIncome
{
	public int Id { get; set; }

	public int? IdFund { get; set; }

	public int? IdParty { get; set; }

	public int? IdAccountant { get; set; }

	public int? IdTournament { get; set; }

	public int? IdSponorDetail { get; set; }

	public bool? IsDone { get; set; } = false;

	public bool? Type { get; set; }

	public int? IdReason { get; set; }

	public short? DaysOverdue { get; set; } = 0;

	public bool? Status { get; set; } = false;
	[DataType(DataType.Date)]

	public DateTime? CreatedDate { get; set; }

	public virtual Fund? IdFundNavigation { get; set; }

	public virtual Member? IdPartyNavigation { get; set; }

	public virtual Reason? IdReasonNavigation { get; set; }

	public virtual Tournament? IdTournamentNavigation { get; set; }

	public virtual Sponor? IdSponorDetailNavigation { get; set; }

	//public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();

	// New property to hold the accountant's name
	[NotMapped]
	public string? AccountantName { get; set; }

	[Required]
	[Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0.")]
	public decimal Amount { get; set; }
	 public virtual ICollection<Award> Awards { get; set; } = new List<Award>(); // Thêm dòng này
}