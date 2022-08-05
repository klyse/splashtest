using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using web.Models;

#nullable disable

namespace web.Migrations
{
    public partial class TestStepsIsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ICollection<TestStep>>(
                name: "TestSteps",
                table: "Tests",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(ICollection<TestStep>),
                oldType: "jsonb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ICollection<TestStep>>(
                name: "TestSteps",
                table: "Tests",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(ICollection<TestStep>),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}