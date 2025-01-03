using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Match
{
    public int IdMatch { get; set; }

    public int? IdTournament { get; set; }

    public int? IdMemberOne { get; set; }

    public int? IdMemberTwo { get; set; }

    public DateTime? TimeStart { get; set; }

    public int? IdGroupstage { get; set; }

    public int? IdMemberWin { get; set; }

    public bool? Status { get; set; }

    public virtual Groupstage? IdGroupstageNavigation { get; set; }

    public virtual Player? IdMemberOneNavigation { get; set; }

    public virtual Player? IdMemberTwoNavigation { get; set; }

    public virtual Player? IdMemberWinNavigation { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }

    public virtual ICollection<Set> Sets { get; set; } = new List<Set>();
}
