#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations;

public partial class AddName : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            "Result",
            "Tests",
            "Name");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            "Name",
            "Tests",
            "Result");
    }
}