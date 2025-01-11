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
    [Display(Name = "Id Quyền cha quản lý")]
    public int ? IdPerParent { get; set; }
    [Display(Name = "Quyền cha quản lý")]
    public virtual Permission? ParentPermission { get; set; }
    public virtual ICollection<Permission> SubPermissions { get; set; } = new List<Permission>();
    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
