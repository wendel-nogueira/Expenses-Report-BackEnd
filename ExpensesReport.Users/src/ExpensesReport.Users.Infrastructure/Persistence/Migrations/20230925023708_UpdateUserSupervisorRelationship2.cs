using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesReport.Users.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSupervisorRelationship2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_SupervisorId1",
                table: "UserSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSupervisor_Users_UserId",
                table: "UserSupervisor");

            migrationBuilder.DropIndex(
                name: "IX_UserSupervisor_SupervisorId1",
                table: "UserSupervisor");

            migrationBuilder.RenameColumn(
                name: "SupervisorId1",
                table: "UserSupervisor",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSupervisor_User",
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
                name: "FK_UserSupervisor_User",
                table: "UserSupervisor");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserSupervisor",
                newName: "SupervisorId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserSupervisor_SupervisorId1",
                table: "UserSupervisor",
                column: "SupervisorId1");

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
    }
}
