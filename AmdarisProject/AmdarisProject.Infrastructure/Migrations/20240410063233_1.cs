using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmdarisProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinAt = table.Column<long>(type: "bigint", nullable: true),
                    DurationInSeconds = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    BreakInSeconds = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    GameType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompetitorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamSize = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.Id);
                    table.CheckConstraint("CK_competitor_type", "[CompetitorType] = 'Player' AND [TeamSize] = NULL OR [CompetitorType] = 'Team' AND [TeamSize] <> NULL");
                    table.CheckConstraint("CK_win_rules", "[WinAt] <> NULL OR ([DurationInSeconds] <> NULL AND [BreakInSeconds] <> NULL)");
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    TeamSize = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageLevel = table.Column<int>(type: "int", nullable: false),
                    TournamentCompetitionId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stage_Competitions_TournamentCompetitionId",
                        column: x => x.TournamentCompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionCompetitor",
                columns: table => new
                {
                    CompetitionsId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CompetitorsId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionCompetitor", x => new { x.CompetitionsId, x.CompetitorsId });
                    table.ForeignKey(
                        name: "FK_CompetitionCompetitor_Competitions_CompetitionsId",
                        column: x => x.CompetitionsId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionCompetitor_Competitors_CompetitorsId",
                        column: x => x.CompetitorsId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTeam",
                columns: table => new
                {
                    PlayersId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    TeamsId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeam", x => new { x.PlayersId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Competitors_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Competitors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTeam_Competitors_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompetitorOneId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CompetitorTwoId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CompetitionId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StageId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Competitors_CompetitorOneId",
                        column: x => x.CompetitorOneId,
                        principalTable: "Competitors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Competitors_CompetitorTwoId",
                        column: x => x.CompetitorTwoId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    MatchId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    PlayerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Competitors_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Points_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionCompetitor_CompetitorsId",
                table: "CompetitionCompetitor",
                column: "CompetitorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitionId",
                table: "Matches",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitorOneId",
                table: "Matches",
                column: "CompetitorOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CompetitorTwoId",
                table: "Matches",
                column: "CompetitorTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_StageId",
                table: "Matches",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeam_TeamsId",
                table: "PlayerTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_MatchId",
                table: "Points",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_PlayerId",
                table: "Points",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_TournamentCompetitionId",
                table: "Stage",
                column: "TournamentCompetitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionCompetitor");

            migrationBuilder.DropTable(
                name: "PlayerTeam");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropTable(
                name: "Competitions");
        }
    }
}
