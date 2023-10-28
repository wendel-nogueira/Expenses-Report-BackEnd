using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesReport.Projects.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Projects",
                newName: "DepartamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartamentId",
                table: "Projects",
                newName: "DepartmentId");
        }
    }
}
