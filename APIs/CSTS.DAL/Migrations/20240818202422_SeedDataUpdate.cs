using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSTS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("725c2c65-2fc0-45ec-b28f-fc56f268c225"),
                columns: new[] { "Image", "Password" },
                values: new object[] { "/Image/Manager.webp", ".�Ra�ˈof)h�#�s�����9��e�" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("725c2c65-2fc0-45ec-b28f-fc56f268c225"),
                columns: new[] { "Image", "Password" },
                values: new object[] { null, "123456" });
        }
    }
}
