using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Player
{
    public int IdPlayer { get; set; }

    public int IdTournament { get; set; }

    public int IdMember { get; set; }

    public short? Score { get; set; }

    public bool? Status { get; set; }

    public virtual Member IdMemberNavigation { get; set; } = null!;

    public virtual Tournament IdTournamentNavigation { get; set; } = null!;

    public virtual ICollection<Match> MatchIdMemberOneNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchIdMemberTwoNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchIdMemberWinNavigations { get; set; } = new List<Match>();
}
