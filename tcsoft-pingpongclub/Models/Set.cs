using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Set
{
    public int IdSet { get; set; }

    public int IdMatch { get; set; }

    public string? Ratio { get; set; }

    public string? SetName { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public bool? Status { get; set; }

    public virtual Match IdMatchNavigation { get; set; } = null!;
}
