using Microsoft.EntityFrameworkCore.Migrations;

namespace Prism.Migrations
{
    public partial class NewIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_SimHash",
                table: "NewsArticles");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_Group",
                table: "NewsArticles",
                column: "Group");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_Group",
                table: "NewsArticles");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_SimHash",
                table: "NewsArticles",
                column: "SimHash");
        }
    }
}
