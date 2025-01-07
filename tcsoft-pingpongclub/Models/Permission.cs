using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Permission
{
    public int IdPermission { get; set; }

    [Display(Name = "Tên quyền ")]
    public string? NamePermission { get; set; }
    [Display(Name = "Đường dẫn url")]
    public string? Url { get; set; }
    [Display(Name = "Trạng thái")]
    public bool? Status { get; set; }

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
