using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class ExpenseAndIncome
{
    public int Id { get; set; }

    public int? IdFund { get; set; }

    public int? IdParty { get; set; }

    public int? IdAccountant { get; set; }

    public bool? IsDone { get; set; }

    public bool? Type { get; set; }

    public int? IdReason { get; set; }

    public short? DaysOverdue { get; set; }

    public bool? Status { get; set; }

    public virtual Fund? IdFundNavigation { get; set; }

    public virtual Member? IdPartyNavigation { get; set; }

    public virtual Reason? IdReasonNavigation { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
