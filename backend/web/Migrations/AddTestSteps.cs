using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using web.Models;

#nullable disable

namespace web.Migrations
{
    public partial class AddTestSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestCode",
                table: "Tests");

            migrationBuilder.AddColumn<ICollection<TestStep>>(
                name: "TestSteps",
                table: "Tests",
                type: "jsonb",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestSteps",
                table: "Tests");

            migrationBuilder.AddColumn<string>(
                name: "TestCode",
                table: "Tests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}