using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesReport.Departaments.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.RenameIndex(
                name: "IX_User_DepartamentId",
                table: "Users",
                newName: "IX_Users_DepartamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Manager_DepartamentId",
                table: "Managers",
                newName: "IX_Managers_DepartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DepartamentId",
                table: "User",
                newName: "IX_User_DepartamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_DepartamentId",
                table: "Manager",
                newName: "IX_Manager_DepartamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "Id");
        }
    }
}
