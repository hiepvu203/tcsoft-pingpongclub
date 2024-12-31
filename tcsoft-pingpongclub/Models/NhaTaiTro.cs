using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class NhaTaiTro
{
    public int IdSponor { get; set; }

    public string? NameSponer { get; set; }

    public string? UrlLogo { get; set; }

    [NotMapped] // This makes sure it doesn't get saved to the database
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
