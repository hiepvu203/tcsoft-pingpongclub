using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Tournament
{
    public int IdTournament { get; set; }

    public string? TournamentName { get; set; }

    public bool? Type { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public string? UrlImage { get; set; }

    public short? Amount { get; set; }

    public int? RankStart { get; set; }

    public int? RankEnd { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<Groupstage> Groupstages { get; set; } = new List<Groupstage>();

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual Level? RankEndNavigation { get; set; }

    public virtual Level? RankStartNavigation { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
