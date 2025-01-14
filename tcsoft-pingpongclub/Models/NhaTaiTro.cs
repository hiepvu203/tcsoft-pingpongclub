using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class NhaTaiTro
{
    [Key]
    public int IdSponor { get; set; }
    [Required(ErrorMessage = "Tên nhà tài trợ không được để trống.")]
    public string? NameSponer { get; set; }
    [Required(ErrorMessage = "Đường dẫn ảnh không được để trống.")]
    public string? UrlLogo { get; set; }

    public bool? Status { get; set; } = false;

    [NotMapped] // This makes sure it doesn't get saved to the database
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Sponor> Sponors { get; set; } = new List<Sponor>();
}
