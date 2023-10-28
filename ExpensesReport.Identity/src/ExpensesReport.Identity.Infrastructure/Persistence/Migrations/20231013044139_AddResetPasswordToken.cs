using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpensesReport.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "220413e4-d834-4d3b-81ac-fc9d87387a1b", null, "Manager", "MANAGER" },
                    { "aeb3013c-0bca-4003-990d-d1b788eabad0", null, "Accountant", "ACCOUNTANT" },
                    { "d264fa9c-989d-4d59-9674-3bb25e9e5e80", null, "FieldStaff", "FIELDSTAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "220413e4-d834-4d3b-81ac-fc9d87387a1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aeb3013c-0bca-4003-990d-d1b788eabad0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d264fa9c-989d-4d59-9674-3bb25e9e5e80");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "AspNetUsers");

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
    }
}
