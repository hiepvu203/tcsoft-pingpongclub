using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class NhaTaiTro
{
    public int IdSponor { get; set; }

    public string? NameSponer { get; set; }

    public string? UrlLogo { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
