using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcsoft_pingpongclub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fund",
                columns: table => new
                {
                    idFund = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fundName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk1", x => x.idFund);
                });

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    idLevel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    levelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    scoreStart = table.Column<int>(type: "int", nullable: true),
                    scoreEnd = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk3", x => x.idLevel);
                });

            migrationBuilder.CreateTable(
                name: "NhaTaiTro",
                columns: table => new
                {
                    idSponor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameSponer = table.Column<string>(type: "text", nullable: true),
                    urlLogo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PKidSponor", x => x.idSponor);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    idPermission = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    namePermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkPermission", x => x.idPermission);
                });

            migrationBuilder.CreateTable(
                name: "Reason",
                columns: table => new
                {
                    idReason = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reasonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<bool>(type: "bit", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkReason", x => x.idReason);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    idRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRole", x => x.idRole);
                });

            migrationBuilder.CreateTable(
                name: "ScoreCal",
                columns: table => new
                {
                    idScoreCal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ptsMin = table.Column<short>(type: "smallint", nullable: true),
                    ptsMax = table.Column<short>(type: "smallint", nullable: true),
                    ptsSameRankWin = table.Column<short>(type: "smallint", nullable: true),
                    ptsHighRankWin = table.Column<short>(type: "smallint", nullable: true),
                    ptsSameRankDef = table.Column<short>(type: "smallint", nullable: true),
                    ptsHighRankDef = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkScoreCal", x => x.idScoreCal);
                });

            migrationBuilder.CreateTable(
                name: "HadicapTable",
                columns: table => new
                {
                    idHadicap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idHighLevel = table.Column<int>(type: "int", nullable: true),
                    idLowLevel = table.Column<int>(type: "int", nullable: true),
                    hadicap = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkHadicap", x => x.idHadicap);
                    table.ForeignKey(
                        name: "FK_HadicapTable_Level",
                        column: x => x.idHighLevel,
                        principalTable: "Level",
                        principalColumn: "idLevel");
                    table.ForeignKey(
                        name: "FK_HadicapTable_Level1",
                        column: x => x.idLowLevel,
                        principalTable: "Level",
                        principalColumn: "idLevel");
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    idTournament = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tournamentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<bool>(type: "bit", nullable: true),
                    timeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    timeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    urlImage = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    amount = table.Column<short>(type: "smallint", nullable: true),
                    rankStart = table.Column<int>(type: "int", nullable: true),
                    rankEnd = table.Column<int>(type: "int", nullable: true),
                    Infor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkidTournament", x => x.idTournament);
                    table.ForeignKey(
                        name: "FK_Tournament_Level",
                        column: x => x.rankStart,
                        principalTable: "Level",
                        principalColumn: "idLevel");
                    table.ForeignKey(
                        name: "FK_Tournament_Level1",
                        column: x => x.rankEnd,
                        principalTable: "Level",
                        principalColumn: "idLevel");
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    idMember = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberName = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    emaill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    idLevel = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    linkAvatar = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    idRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkMember", x => x.idMember);
                    table.ForeignKey(
                        name: "FK_Member_Level",
                        column: x => x.idLevel,
                        principalTable: "Level",
                        principalColumn: "idLevel");
                    table.ForeignKey(
                        name: "FK_Member_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "idRole");
                });

            migrationBuilder.CreateTable(
                name: "Permission_Role",
                columns: table => new
                {
                    idPerRo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idRole = table.Column<int>(type: "int", nullable: false),
                    idPermission = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkPerRo", x => x.idPerRo);
                    table.ForeignKey(
                        name: "FK_Permission_Role_Permission",
                        column: x => x.idPermission,
                        principalTable: "Permission",
                        principalColumn: "idPermission");
                    table.ForeignKey(
                        name: "FK_Permission_Role_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "idRole");
                });

            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    idAward = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTournament = table.Column<int>(type: "int", nullable: true),
                    iOrder = table.Column<short>(type: "smallint", nullable: true),
                    money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    score = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award_idAward", x => x.idAward);
                    table.ForeignKey(
                        name: "FK_Award_Tournament",
                        column: x => x.idTournament,
                        principalTable: "Tournament",
                        principalColumn: "idTournament");
                });

            migrationBuilder.CreateTable(
                name: "Groupstage",
                columns: table => new
                {
                    idGroupstage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: true),
                    iOrder = table.Column<short>(type: "smallint", nullable: true),
                    idTournament = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkRound", x => x.idGroupstage);
                    table.ForeignKey(
                        name: "FK_Groupstage_Tournament",
                        column: x => x.idTournament,
                        principalTable: "Tournament",
                        principalColumn: "idTournament");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseAndIncome",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idFund = table.Column<int>(type: "int", nullable: true),
                    idParty = table.Column<int>(type: "int", nullable: true),
                    idAccountant = table.Column<int>(type: "int", nullable: true),
                    isDone = table.Column<bool>(type: "bit", nullable: true),
                    type = table.Column<bool>(type: "bit", nullable: true),
                    idReason = table.Column<int>(type: "int", nullable: true),
                    daysOverdue = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_ExpenseAndIncome_Fund",
                        column: x => x.idFund,
                        principalTable: "Fund",
                        principalColumn: "idFund");
                    table.ForeignKey(
                        name: "FK_ExpenseAndIncome_Member",
                        column: x => x.idParty,
                        principalTable: "Member",
                        principalColumn: "idMember");
                    table.ForeignKey(
                        name: "FK_ExpenseAndIncome_Reason",
                        column: x => x.idReason,
                        principalTable: "Reason",
                        principalColumn: "idReason");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    idPlayer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTournament = table.Column<int>(type: "int", nullable: false),
                    idMember = table.Column<int>(type: "int", nullable: false),
                    score = table.Column<short>(type: "smallint", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkPlayer", x => x.idPlayer);
                    table.ForeignKey(
                        name: "FK_Player_Member",
                        column: x => x.idMember,
                        principalTable: "Member",
                        principalColumn: "idMember");
                    table.ForeignKey(
                        name: "FK_Player_Tournament",
                        column: x => x.idTournament,
                        principalTable: "Tournament",
                        principalColumn: "idTournament");
                });

            migrationBuilder.CreateTable(
                name: "Sponor",
                columns: table => new
                {
                    idSponorTour = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idIncome = table.Column<int>(type: "int", nullable: true),
                    money = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    idTournament = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    idSponor = table.Column<int>(type: "int", nullable: true),
                    other = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkSponor", x => x.idSponorTour);
                    table.ForeignKey(
                        name: "FK_Sponor_ExpenseAndIncome",
                        column: x => x.idIncome,
                        principalTable: "ExpenseAndIncome",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Sponor_NhaTaiTro",
                        column: x => x.idSponor,
                        principalTable: "NhaTaiTro",
                        principalColumn: "idSponor");
                    table.ForeignKey(
                        name: "FK_Sponor_Tournament",
                        column: x => x.idTournament,
                        principalTable: "Tournament",
                        principalColumn: "idTournament");
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    idMatch = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTournament = table.Column<int>(type: "int", nullable: true),
                    idMemberOne = table.Column<int>(type: "int", nullable: true),
                    idMemberTwo = table.Column<int>(type: "int", nullable: true),
                    timeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    idGroupstage = table.Column<int>(type: "int", nullable: true),
                    idMemberWin = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkMatch", x => x.idMatch);
                    table.ForeignKey(
                        name: "FK_Match_Groupstage",
                        column: x => x.idGroupstage,
                        principalTable: "Groupstage",
                        principalColumn: "idGroupstage");
                    table.ForeignKey(
                        name: "FK_Match_Player",
                        column: x => x.idMemberOne,
                        principalTable: "Player",
                        principalColumn: "idPlayer");
                    table.ForeignKey(
                        name: "FK_Match_Player1",
                        column: x => x.idMemberTwo,
                        principalTable: "Player",
                        principalColumn: "idPlayer");
                    table.ForeignKey(
                        name: "FK_Match_Player2",
                        column: x => x.idMemberWin,
                        principalTable: "Player",
                        principalColumn: "idPlayer");
                    table.ForeignKey(
                        name: "FK_Match_Tournament",
                        column: x => x.idTournament,
                        principalTable: "Tournament",
                        principalColumn: "idTournament");
                });

            migrationBuilder.CreateTable(
                name: "Set",
                columns: table => new
                {
                    idSet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMatch = table.Column<int>(type: "int", nullable: false),
                    idWinner = table.Column<int>(type: "int", nullable: false),
                    ratio = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    setName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    timeStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    timeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkSet", x => x.idSet);
                    table.ForeignKey(
                        name: "FK_Set_Match",
                        column: x => x.idMatch,
                        principalTable: "Match",
                        principalColumn: "idMatch");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Award_idTournament",
                table: "Award",
                column: "idTournament");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseAndIncome_idFund",
                table: "ExpenseAndIncome",
                column: "idFund");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseAndIncome_idParty",
                table: "ExpenseAndIncome",
                column: "idParty");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseAndIncome_idReason",
                table: "ExpenseAndIncome",
                column: "idReason");

            migrationBuilder.CreateIndex(
                name: "IX_Groupstage_idTournament",
                table: "Groupstage",
                column: "idTournament");

            migrationBuilder.CreateIndex(
                name: "IX_HadicapTable_idHighLevel",
                table: "HadicapTable",
                column: "idHighLevel");

            migrationBuilder.CreateIndex(
                name: "IX_HadicapTable_idLowLevel",
                table: "HadicapTable",
                column: "idLowLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Match_idGroupstage",
                table: "Match",
                column: "idGroupstage");

            migrationBuilder.CreateIndex(
                name: "IX_Match_idMemberOne",
                table: "Match",
                column: "idMemberOne");

            migrationBuilder.CreateIndex(
                name: "IX_Match_idMemberTwo",
                table: "Match",
                column: "idMemberTwo");

            migrationBuilder.CreateIndex(
                name: "IX_Match_idMemberWin",
                table: "Match",
                column: "idMemberWin");

            migrationBuilder.CreateIndex(
                name: "IX_Match_idTournament",
                table: "Match",
                column: "idTournament");

            migrationBuilder.CreateIndex(
                name: "constraint_name1",
                table: "Member",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Member_idLevel",
                table: "Member",
                column: "idLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Member_idRole",
                table: "Member",
                column: "idRole");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_idPermission",
                table: "Permission_Role",
                column: "idPermission");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_idRole",
                table: "Permission_Role",
                column: "idRole");

            migrationBuilder.CreateIndex(
                name: "IX_Player_idMember",
                table: "Player",
                column: "idMember");

            migrationBuilder.CreateIndex(
                name: "IX_Player_idTournament",
                table: "Player",
                column: "idTournament");

            migrationBuilder.CreateIndex(
                name: "IX_Set_idMatch",
                table: "Set",
                column: "idMatch");

            migrationBuilder.CreateIndex(
                name: "IX_Sponor_idIncome",
                table: "Sponor",
                column: "idIncome");

            migrationBuilder.CreateIndex(
                name: "IX_Sponor_idSponor",
                table: "Sponor",
                column: "idSponor");

            migrationBuilder.CreateIndex(
                name: "IX_Sponor_idTournament",
                table: "Sponor",
                column: "idTournament");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_rankEnd",
                table: "Tournament",
                column: "rankEnd");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_rankStart",
                table: "Tournament",
                column: "rankStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Award");

            migrationBuilder.DropTable(
                name: "HadicapTable");

            migrationBuilder.DropTable(
                name: "Permission_Role");

            migrationBuilder.DropTable(
                name: "ScoreCal");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.DropTable(
                name: "Sponor");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "ExpenseAndIncome");

            migrationBuilder.DropTable(
                name: "NhaTaiTro");

            migrationBuilder.DropTable(
                name: "Groupstage");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Fund");

            migrationBuilder.DropTable(
                name: "Reason");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Level");
        }
    }
}
