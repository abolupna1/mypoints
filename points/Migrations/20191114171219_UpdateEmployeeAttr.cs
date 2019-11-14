using Microsoft.EntityFrameworkCore.Migrations;

namespace points.Migrations
{
    public partial class UpdateEmployeeAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JopName",
                table: "Employees",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "JopType",
                table: "Employees",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JopName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JopType",
                table: "Employees");
        }
    }
}
