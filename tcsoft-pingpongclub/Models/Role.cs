using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class Role
{
    public int IdRole { get; set; }

    [Display(Name = "Tên loại tài khoản")]
    public string? NameRole { get; set; }
    [Display(Name = "Trạng thái")]
    public bool? Status { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
