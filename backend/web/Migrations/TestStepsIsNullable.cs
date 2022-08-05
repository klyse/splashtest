#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using web.Models;

namespace web.Migrations;

public partial class TestStepsIsNullable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<ICollection<TestStep>>(
            "TestSteps",
            "Tests",
            "jsonb",
            nullable: true,
            oldClrType: typeof(ICollection<TestStep>),
            oldType: "jsonb");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<ICollection<TestStep>>(
            "TestSteps",
            "Tests",
            "jsonb",
            nullable: false,
            oldClrType: typeof(ICollection<TestStep>),
            oldType: "jsonb",
            oldNullable: true);
    }
}