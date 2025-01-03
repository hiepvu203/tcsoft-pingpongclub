using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class PermissionRole
{
    public int IdPerRo { get; set; }

    public int IdRole { get; set; }

    public int IdPermission { get; set; }

    public bool? Status { get; set; }

    public virtual Permission IdPermissionNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
