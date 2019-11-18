using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace points.Migrations
{
    public partial class BusinessAndAchievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessAndAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    TimesOfEvaluationAndPerformanceId = table.Column<int>(nullable: false),
                    TheWork = table.Column<string>(maxLength: 150, nullable: false),
                    TheWorkDescription = table.Column<string>(nullable: false),
                    WorkDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    NotesForWorkDelayed = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    QuicklyPerformTheTask = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessAndAchievements", x => new { x.Id, x.EmployeeId, x.TimesOfEvaluationAndPerformanceId });
                    table.UniqueConstraint("AK_BusinessAndAchievements_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessAndAchievements_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessAndAchievements_TimesOfEvaluationAndPerformances_TimesOfEvaluationAndPerformanceId",
                        column: x => x.TimesOfEvaluationAndPerformanceId,
                        principalTable: "TimesOfEvaluationAndPerformances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAndAchievements_EmployeeId",
                table: "BusinessAndAchievements",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAndAchievements_TimesOfEvaluationAndPerformanceId",
                table: "BusinessAndAchievements",
                column: "TimesOfEvaluationAndPerformanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessAndAchievements");
        }
    }
}
