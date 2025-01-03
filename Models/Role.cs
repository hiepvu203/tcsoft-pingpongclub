using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string? NameRole { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
