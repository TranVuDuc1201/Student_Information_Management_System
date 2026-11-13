using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIMS.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticationAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Faculties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SystemAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAdmins", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Faculties",
                keyColumn: "FacultyId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Email", "PasswordHash" },
                values: new object[] { "vancem@fpt.edu.vn", "$2a$11$ABCDEFGHIJKLMNOPQRSTUOF8KfyBfX4omEVVVTzh8y1AdC6BtobkC" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "$2a$11$ABCDEFGHIJKLMNOPQRSTUOF8KfyBfX4omEVVVTzh8y1AdC6BtobkC");

            migrationBuilder.InsertData(
                table: "SystemAdmins",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), "adminduc000@gmail.com", "Duc Tran (System Admin)", "$2a$11$ABCDEFGHIJKLMNOPQRSTUOKlySru39VvENm.dVpzLdZEClj4puKeC" });

            migrationBuilder.CreateIndex(
                name: "IX_SystemAdmins_Email",
                table: "SystemAdmins",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemAdmins");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Faculties");
        }
    }
}
