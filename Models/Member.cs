using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class Member
{
    public int IdMember { get; set; }

    public string? MemberName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Emaill { get; set; }

    public bool? Gender { get; set; }

    public int? IdLevel { get; set; }

    public bool? Status { get; set; }

    public string? LinkAvatar { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? IdRole { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();

    public virtual Level? IdLevelNavigation { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
