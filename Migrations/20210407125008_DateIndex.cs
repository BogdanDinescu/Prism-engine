using Microsoft.EntityFrameworkCore.Migrations;

namespace Prism.Migrations
{
    public partial class DateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_CreateDate",
                table: "NewsArticles",
                column: "CreateDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_CreateDate",
                table: "NewsArticles");
        }
    }
}
