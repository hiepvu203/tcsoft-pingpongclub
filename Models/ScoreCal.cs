using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class ScoreCal
{
    public int IdScoreCal { get; set; }

    public short? PtsMin { get; set; }

    public short? PtsMax { get; set; }

    public short? PtsSameRankWin { get; set; }

    public short? PtsHighRankWin { get; set; }

    public short? PtsSameRankDef { get; set; }

    public short? PtsHighRankDef { get; set; }

    public bool? Status { get; set; }
}
