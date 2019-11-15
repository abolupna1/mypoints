using Microsoft.EntityFrameworkCore.Migrations;

namespace points.Migrations
{
    public partial class TimesOfEvaluationAndPerformanceUpdateTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TimesOfEvaluationAndPerformances",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "TimesOfEvaluationAndPerformances");
        }
    }
}
