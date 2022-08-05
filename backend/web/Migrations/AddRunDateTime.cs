#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations;

public partial class AddRunDateTime : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            "RunDateTime",
            "Runs",
            "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "RunDateTime",
            "Runs");
    }
}