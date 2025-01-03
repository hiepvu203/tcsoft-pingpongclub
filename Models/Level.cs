using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Level
{
    public int IdLevel { get; set; }

    public string? LevelName { get; set; }

    public int? ScoreStart { get; set; }

    public int? ScoreEnd { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<HadicapTable> HadicapTableIdHighLevelNavigations { get; set; } = new List<HadicapTable>();

    public virtual ICollection<HadicapTable> HadicapTableIdLowLevelNavigations { get; set; } = new List<HadicapTable>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<Tournament> TournamentRankEndNavigations { get; set; } = new List<Tournament>();

    public virtual ICollection<Tournament> TournamentRankStartNavigations { get; set; } = new List<Tournament>();
}
