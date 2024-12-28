using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Award
{
    public int IdAward { get; set; }

    public int? IdTournament { get; set; }

    public short? IOrder { get; set; }

    public decimal? Money { get; set; }

    public short? Score { get; set; }

    public bool? Status { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }
}
