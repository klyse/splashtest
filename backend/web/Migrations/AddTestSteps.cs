#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using web.Models;

namespace web.Migrations;

public partial class AddTestSteps : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "TestCode",
            "Tests");

        migrationBuilder.AddColumn<ICollection<TestStep>>(
            "TestSteps",
            "Tests",
            "jsonb",
            nullable: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "TestSteps",
            "Tests");

        migrationBuilder.AddColumn<string>(
            "TestCode",
            "Tests",
            "text",
            nullable: false,
            defaultValue: "");
    }
}