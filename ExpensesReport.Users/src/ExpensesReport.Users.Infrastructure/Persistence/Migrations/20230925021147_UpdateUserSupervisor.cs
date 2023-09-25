using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesReport.Users.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_UserId",
                table: "UserSupervisor");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserSupervisor");

            migrationBuilder.CreateIndex(
                name: "IX_UserSupervisor_SupervisorId",
                table: "UserSupervisor",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId",
                table: "UserSupervisor",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId",
                table: "UserSupervisor");

            migrationBuilder.DropIndex(
                name: "IX_UserSupervisor_SupervisorId",
                table: "UserSupervisor");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserSupervisor",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Users_UserId",
                table: "UserSupervisor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
