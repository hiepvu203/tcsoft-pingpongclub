using System;
using System.Collections.Generic;

namespace tcsoft_pingpongclub.Models;

public partial class Sponor
{
    public int IdSponorTour { get; set; }

    public int? IdIncome { get; set; }

    public decimal? Money { get; set; }

    public int? IdTournament { get; set; }

    public bool? Status { get; set; }

    public int? IdSponor { get; set; }

    public string? Other { get; set; }

    public virtual ExpenseAndIncome? IdIncomeNavigation { get; set; }

    public virtual NhaTaiTro? IdSponorNavigation { get; set; }

    public virtual Tournament? IdTournamentNavigation { get; set; }
}
