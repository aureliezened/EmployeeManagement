using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebUsers_Employees_employeeId",
                table: "WebUsers");

            migrationBuilder.DropIndex(
                name: "IX_WebUsers_employeeId",
                table: "WebUsers");

            migrationBuilder.DropColumn(
                name: "employeeId",
                table: "WebUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "employeeId",
                table: "WebUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebUsers_employeeId",
                table: "WebUsers",
                column: "employeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebUsers_Employees_employeeId",
                table: "WebUsers",
                column: "employeeId",
                principalTable: "Employees",
                principalColumn: "employeeId");
        }
    }
}
