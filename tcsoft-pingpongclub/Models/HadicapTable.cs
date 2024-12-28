using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class HadicapTable
{
    public int IdHadicap { get; set; }

    public int? IdHighLevel { get; set; }

    public int? IdLowLevel { get; set; }

    public short? Hadicap { get; set; }

    public bool? Status { get; set; }

    public virtual Level? IdHighLevelNavigation { get; set; }

    public virtual Level? IdLowLevelNavigation { get; set; }
}
