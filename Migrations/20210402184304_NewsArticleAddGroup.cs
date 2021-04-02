using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prism.Migrations
{
    public partial class NewsArticleAddGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "NewsArticles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "NewsArticles");
        }
    }
}
