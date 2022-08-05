#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations;

public partial class InitDb : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Runs",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                State = table.Column<int>("integer", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Runs", x => x.Id); });

        migrationBuilder.CreateTable(
            "Tests",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                Result = table.Column<string>("text", nullable: false),
                TestCode = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Tests", x => x.Id); });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Runs");

        migrationBuilder.DropTable(
            "Tests");
    }
}