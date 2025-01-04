using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models;

public partial class PermissionRole
{
    [Display(Name = "ID")]
    public int IdPerRo { get; set; }
    [Display(Name = "ID của loại tài khoản")]
    public int IdRole { get; set; }
    [Display(Name = "ID của quyền")]
    public int IdPermission { get; set; }
    [Display(Name = "Trạng thái")]
    public bool? Status { get; set; }
    [Display(Name = "ID của quyền")]
    public virtual Permission IdPermissionNavigation { get; set; } = null!;
    [Display(Name = "ID của loại tài khoản")]
    public virtual Role IdRoleNavigation { get; set; } = null!;
}
