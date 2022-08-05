#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations;

public partial class AddedRuns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            "TestId",
            "Runs",
            "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            "IX_Runs_TestId",
            "Runs",
            "TestId");

        migrationBuilder.AddForeignKey(
            "FK_Runs_Tests_TestId",
            "Runs",
            "TestId",
            "Tests",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            "FK_Runs_Tests_TestId",
            "Runs");

        migrationBuilder.DropIndex(
            "IX_Runs_TestId",
            "Runs");

        migrationBuilder.DropColumn(
            "TestId",
            "Runs");
    }
}