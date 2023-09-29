using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpensesReport.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoleSeedsId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Accountant");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "FieldStaff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Manager");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44b68756-f460-4bb0-b485-00fa2045ad89", null, "Manager", "MANAGER" },
                    { "ca648175-b11d-414e-abdd-e8dd4aa75084", null, "FieldStaff", "FIELDSTAFF" },
                    { "cd48bf1b-0f96-4b2a-8d30-1cf86939d58b", null, "Accountant", "ACCOUNTANT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44b68756-f460-4bb0-b485-00fa2045ad89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca648175-b11d-414e-abdd-e8dd4aa75084");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd48bf1b-0f96-4b2a-8d30-1cf86939d58b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "Accountant", null, "Accountant", "ACCOUNTANT" },
                    { "FieldStaff", null, "FieldStaff", "FIELDSTAFF" },
                    { "Manager", null, "Manager", "MANAGER" }
                });
        }
    }
}
