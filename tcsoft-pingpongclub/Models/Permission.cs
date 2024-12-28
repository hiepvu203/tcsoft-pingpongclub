using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Permission
{
    public int IdPermission { get; set; }

    public string? NamePermission { get; set; }

    public string? Url { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
