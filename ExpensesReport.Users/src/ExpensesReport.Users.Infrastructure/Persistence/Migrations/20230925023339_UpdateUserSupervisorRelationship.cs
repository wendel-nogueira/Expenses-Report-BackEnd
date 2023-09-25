using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesReport.Users.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSupervisorRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId",
                table: "UserSupervisor");

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId1",
                table: "UserSupervisor",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserSupervisor_SupervisorId1",
                table: "UserSupervisor",
                column: "SupervisorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Supervisor",
                table: "UserSupervisor",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId1",
                table: "UserSupervisor",
                column: "SupervisorId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Users_UserId",
                table: "UserSupervisor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Supervisor",
                table: "UserSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId1",
                table: "UserSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_UserId",
                table: "UserSupervisor");

            migrationBuilder.DropIndex(
                name: "IX_UserSupervisor_SupervisorId1",
                table: "UserSupervisor");

            migrationBuilder.DropColumn(
                name: "SupervisorId1",
                table: "UserSupervisor");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId",
                table: "UserSupervisor",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
