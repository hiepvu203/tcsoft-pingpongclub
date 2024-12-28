using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Groupstage
{
    public int IdGroupstage { get; set; }

    public string? NameGroup { get; set; }

    public int? Amount { get; set; }

    public short? IOrder { get; set; }

    public int? IdTournament { get; set; }

    public bool? Status { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
