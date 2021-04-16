using Microsoft.EntityFrameworkCore.Migrations;

namespace Prism.Migrations
{
    public partial class CityPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserPreferences",
                type: "varchar(60) CHARACTER SET utf8mb4",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "UserPreferences");
        }
    }
}
