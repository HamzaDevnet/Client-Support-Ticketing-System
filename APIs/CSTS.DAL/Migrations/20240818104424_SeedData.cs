using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSTS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "DateOfBirth", "Email", "FirstName", "Image", "LastName", "MobileNumber", "Password", "RegistrationDate", "UserName", "UserStatus", "UserType" },
                values: new object[] { new Guid("725c2c65-2fc0-45ec-b28f-fc56f268c225"), "Address1", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manger@example.com", "Admin", null, "Admin", "1234567890", "123456", new DateTime(2024, 8, 14, 15, 30, 0, 0, DateTimeKind.Unspecified), "Manger", 1, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("725c2c65-2fc0-45ec-b28f-fc56f268c225"));
        }
    }
}
