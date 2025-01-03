using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace tcsoft_pingpongclub.Models;

public partial class ThuctapKtktcn2024Context : DbContext
{
    public ThuctapKtktcn2024Context()
    {
    }

    public ThuctapKtktcn2024Context(DbContextOptions<ThuctapKtktcn2024Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Award> Awards { get; set; }

    public virtual DbSet<ExpenseAndIncome> ExpenseAndIncomes { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<Groupstage> Groupstages { get; set; }

    public virtual DbSet<HadicapTable> HadicapTables { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<NhaTaiTro> NhaTaiTros { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Reason> Reasons { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ScoreCal> ScoreCals { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Sponor> Sponors { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=thuctap.tcsoft.vn,1444;Initial Catalog=thuctap_ktktcn2024;Persist Security Info=True;User ID=user_thuctap_ktktcn2024;Password=Kpdkzs8uDL56;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Award>(entity =>
        {
            entity.HasKey(e => e.IdAward).HasName("PK_Award_idAward");

            entity.ToTable("Award");

            entity.Property(e => e.IdAward).HasColumnName("idAward");
            entity.Property(e => e.IOrder).HasColumnName("iOrder");
            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.Money)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("money");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdTournamentNavigation).WithMany(p => p.Awards)
                .HasForeignKey(d => d.IdTournament)
                .HasConstraintName("FK_Award_Tournament");
        });

        modelBuilder.Entity<ExpenseAndIncome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk");

            entity.ToTable("ExpenseAndIncome");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DaysOverdue).HasColumnName("daysOverdue");
            entity.Property(e => e.IdAccountant).HasColumnName("idAccountant");
            entity.Property(e => e.IdFund).HasColumnName("idFund");
            entity.Property(e => e.IdParty).HasColumnName("idParty");
            entity.Property(e => e.IdReason).HasColumnName("idReason");
            entity.Property(e => e.IsDone).HasColumnName("isDone");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.IdFundNavigation).WithMany(p => p.ExpenseAndIncomes)
                .HasForeignKey(d => d.IdFund)
                .HasConstraintName("FK_ExpenseAndIncome_Fund");

            entity.HasOne(d => d.IdPartyNavigation).WithMany(p => p.ExpenseAndIncomes)
                .HasForeignKey(d => d.IdParty)
                .HasConstraintName("FK_ExpenseAndIncome_Member");

            entity.HasOne(d => d.IdReasonNavigation).WithMany(p => p.ExpenseAndIncomes)
                .HasForeignKey(d => d.IdReason)
                .HasConstraintName("FK_ExpenseAndIncome_Reason");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.HasKey(e => e.IdFund).HasName("pk1");

            entity.ToTable("Fund");

            entity.Property(e => e.IdFund).HasColumnName("idFund");
            entity.Property(e => e.FundName).HasColumnName("fundName");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total");
        });

        modelBuilder.Entity<Groupstage>(entity =>
        {
            entity.HasKey(e => e.IdGroupstage).HasName("pkRound");

            entity.ToTable("Groupstage");

            entity.Property(e => e.IdGroupstage).HasColumnName("idGroupstage");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.IOrder).HasColumnName("iOrder");
            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.NameGroup).HasColumnName("nameGroup");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdTournamentNavigation).WithMany(p => p.Groupstages)
                .HasForeignKey(d => d.IdTournament)
                .HasConstraintName("FK_Groupstage_Tournament");
        });

        modelBuilder.Entity<HadicapTable>(entity =>
        {
            entity.HasKey(e => e.IdHadicap).HasName("pkHadicap");

            entity.ToTable("HadicapTable");

            entity.Property(e => e.IdHadicap).HasColumnName("idHadicap");
            entity.Property(e => e.Hadicap).HasColumnName("hadicap");
            entity.Property(e => e.IdHighLevel).HasColumnName("idHighLevel");
            entity.Property(e => e.IdLowLevel).HasColumnName("idLowLevel");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdHighLevelNavigation).WithMany(p => p.HadicapTableIdHighLevelNavigations)
                .HasForeignKey(d => d.IdHighLevel)
                .HasConstraintName("FK_HadicapTable_Level");

            entity.HasOne(d => d.IdLowLevelNavigation).WithMany(p => p.HadicapTableIdLowLevelNavigations)
                .HasForeignKey(d => d.IdLowLevel)
                .HasConstraintName("FK_HadicapTable_Level1");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.IdLevel).HasName("pk3");

            entity.ToTable("Level");

            entity.Property(e => e.IdLevel).HasColumnName("idLevel");
            entity.Property(e => e.LevelName)
                .HasMaxLength(50)
                .HasColumnName("levelName");
            entity.Property(e => e.ScoreEnd).HasColumnName("scoreEnd");
            entity.Property(e => e.ScoreStart).HasColumnName("scoreStart");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.IdMatch).HasName("pkMatch");

            entity.ToTable("Match");

            entity.Property(e => e.IdMatch).HasColumnName("idMatch");
            entity.Property(e => e.IdGroupstage).HasColumnName("idGroupstage");
            entity.Property(e => e.IdMemberOne).HasColumnName("idMemberOne");
            entity.Property(e => e.IdMemberTwo).HasColumnName("idMemberTwo");
            entity.Property(e => e.IdMemberWin).HasColumnName("idMemberWin");
            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("timeStart");

            entity.HasOne(d => d.IdGroupstageNavigation).WithMany(p => p.Matches)
                .HasForeignKey(d => d.IdGroupstage)
                .HasConstraintName("FK_Match_Groupstage");

            entity.HasOne(d => d.IdMemberOneNavigation).WithMany(p => p.MatchIdMemberOneNavigations)
                .HasForeignKey(d => d.IdMemberOne)
                .HasConstraintName("FK_Match_Player");

            entity.HasOne(d => d.IdMemberTwoNavigation).WithMany(p => p.MatchIdMemberTwoNavigations)
                .HasForeignKey(d => d.IdMemberTwo)
                .HasConstraintName("FK_Match_Player1");

            entity.HasOne(d => d.IdMemberWinNavigation).WithMany(p => p.MatchIdMemberWinNavigations)
                .HasForeignKey(d => d.IdMemberWin)
                .HasConstraintName("FK_Match_Player2");

            entity.HasOne(d => d.IdTournamentNavigation).WithMany(p => p.Matches)
                .HasForeignKey(d => d.IdTournament)
                .HasConstraintName("FK_Match_Tournament");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.IdMember).HasName("pkMember");

            entity.ToTable("Member");

            entity.HasIndex(e => e.Username, "constraint_name1").IsUnique();

            entity.Property(e => e.IdMember).HasColumnName("idMember");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Emaill).HasColumnName("emaill");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IdLevel).HasColumnName("idLevel");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.LinkAvatar)
                .HasColumnType("text")
                .HasColumnName("linkAvatar");
            entity.Property(e => e.MemberName)
                .HasMaxLength(220)
                .HasColumnName("memberName");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.IdLevelNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.IdLevel)
                .HasConstraintName("FK_Member_Level");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_Member_Role");
        });

        modelBuilder.Entity<NhaTaiTro>(entity =>
        {
            entity.HasKey(e => e.IdSponor).HasName("PKidSponor");

            entity.ToTable("NhaTaiTro");

            entity.Property(e => e.IdSponor).HasColumnName("idSponor");
            entity.Property(e => e.NameSponer)
                .HasColumnType("text")
                .HasColumnName("nameSponer");
            entity.Property(e => e.UrlLogo)
                .HasColumnType("text")
                .HasColumnName("urlLogo");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.IdPermission).HasName("pkPermission");

            entity.ToTable("Permission");

            entity.Property(e => e.IdPermission).HasColumnName("idPermission");
            entity.Property(e => e.NamePermission).HasColumnName("namePermission");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Url)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<PermissionRole>(entity =>
        {
            entity.HasKey(e => e.IdPerRo).HasName("pkPerRo");

            entity.ToTable("Permission_Role");

            entity.Property(e => e.IdPerRo).HasColumnName("idPerRo");
            entity.Property(e => e.IdPermission).HasColumnName("idPermission");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdPermissionNavigation).WithMany(p => p.PermissionRoles)
                .HasForeignKey(d => d.IdPermission)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permission_Role_Permission");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.PermissionRoles)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permission_Role_Role");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.IdPlayer).HasName("pkPlayer");

            entity.ToTable("Player");

            entity.Property(e => e.IdPlayer).HasColumnName("idPlayer");
            entity.Property(e => e.IdMember).HasColumnName("idMember");
            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdMemberNavigation).WithMany(p => p.Players)
                .HasForeignKey(d => d.IdMember)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Member");

            entity.HasOne(d => d.IdTournamentNavigation).WithMany(p => p.Players)
                .HasForeignKey(d => d.IdTournament)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Tournament");
        });

        modelBuilder.Entity<Reason>(entity =>
        {
            entity.HasKey(e => e.IdReason).HasName("pkReason");

            entity.ToTable("Reason");

            entity.Property(e => e.IdReason).HasColumnName("idReason");
            entity.Property(e => e.ReasonName).HasColumnName("reasonName");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("pkRole");

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.NameRole).HasColumnName("nameRole");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<ScoreCal>(entity =>
        {
            entity.HasKey(e => e.IdScoreCal).HasName("pkScoreCal");

            entity.ToTable("ScoreCal");

            entity.Property(e => e.IdScoreCal).HasColumnName("idScoreCal");
            entity.Property(e => e.PtsHighRankDef).HasColumnName("ptsHighRankDef");
            entity.Property(e => e.PtsHighRankWin).HasColumnName("ptsHighRankWin");
            entity.Property(e => e.PtsMax).HasColumnName("ptsMax");
            entity.Property(e => e.PtsMin).HasColumnName("ptsMin");
            entity.Property(e => e.PtsSameRankDef).HasColumnName("ptsSameRankDef");
            entity.Property(e => e.PtsSameRankWin).HasColumnName("ptsSameRankWin");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.IdSet).HasName("pkSet");

            entity.ToTable("Set");

            entity.Property(e => e.IdSet).HasColumnName("idSet");
            entity.Property(e => e.IdMatch).HasColumnName("idMatch");
            entity.Property(e => e.Ratio)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ratio");
            entity.Property(e => e.SetName)
                .HasMaxLength(50)
                .HasColumnName("setName");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("timeEnd");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("timeStart");

            entity.HasOne(d => d.IdMatchNavigation).WithMany(p => p.Sets)
                .HasForeignKey(d => d.IdMatch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Set_Match");
        });

        modelBuilder.Entity<Sponor>(entity =>
        {
            entity.HasKey(e => e.IdSponorTour).HasName("pkSponor");

            entity.ToTable("Sponor");

            entity.Property(e => e.IdSponorTour).HasColumnName("idSponorTour");
            entity.Property(e => e.IdIncome).HasColumnName("idIncome");
            entity.Property(e => e.IdSponor).HasColumnName("idSponor");
            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.Money)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("money");
            entity.Property(e => e.Other).HasColumnName("other");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdIncomeNavigation).WithMany(p => p.Sponors)
                .HasForeignKey(d => d.IdIncome)
                .HasConstraintName("FK_Sponor_ExpenseAndIncome");

            entity.HasOne(d => d.IdSponorNavigation).WithMany(p => p.Sponors)
                .HasForeignKey(d => d.IdSponor)
                .HasConstraintName("FK_Sponor_NhaTaiTro");

            entity.HasOne(d => d.IdTournamentNavigation).WithMany(p => p.Sponors)
                .HasForeignKey(d => d.IdTournament)
                .HasConstraintName("FK_Sponor_Tournament");
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.IdTournament).HasName("pkidTournament");

            entity.ToTable("Tournament");

            entity.Property(e => e.IdTournament).HasColumnName("idTournament");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.RankEnd).HasColumnName("rankEnd");
            entity.Property(e => e.RankStart).HasColumnName("rankStart");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("timeEnd");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("timeStart");
            entity.Property(e => e.TournamentName).HasColumnName("tournamentName");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UrlImage)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("urlImage");

            entity.HasOne(d => d.RankEndNavigation).WithMany(p => p.TournamentRankEndNavigations)
                .HasForeignKey(d => d.RankEnd)
                .HasConstraintName("FK_Tournament_Level1");

            entity.HasOne(d => d.RankStartNavigation).WithMany(p => p.TournamentRankStartNavigations)
                .HasForeignKey(d => d.RankStart)
                .HasConstraintName("FK_Tournament_Level");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
