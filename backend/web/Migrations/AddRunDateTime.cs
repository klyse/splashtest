using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    public partial class AddRunDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RunDateTime",
                table: "Runs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunDateTime",
                table: "Runs");
        }
    }
}
