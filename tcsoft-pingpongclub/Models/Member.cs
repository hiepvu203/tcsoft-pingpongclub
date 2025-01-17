using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tcsoft_pingpongclub.Models;

public partial class Member
{
    public int IdMember { get; set; }
    [Required]

    public string? MemberName { get; set; }
    [Required]

    public string? Address { get; set; }
    [Required]

    public string? Phone { get; set; }
    [Required]

    public string? Emaill { get; set; }
    [Required]

    public bool? Gender { get; set; }
    [Required]

    public int? IdLevel { get; set; }

    public bool? Status { get; set; }
    public string? LinkAvatar { get; set; }
    [Required]

    public string? Username { get; set; }
    [Required]

    public string? Password { get; set; }
    [Required]

    public int? IdRole { get; set; }

    public int? Score { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<ExpenseAndIncome> ExpenseAndIncomes { get; set; } = new List<ExpenseAndIncome>();

    public virtual Level? IdLevelNavigation { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}