using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        private static readonly Guid AdminUserId = new Guid("a0000000-0000-0000-0000-000000000001");
        private static readonly Guid AdminRoleId = new Guid("a0000000-0000-0000-0000-000000000002");

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "Password", "Phone", "BirthDate", "CpfNumber", "Situation" },
                values: new object[]
                {
                    AdminUserId,
                    "Admin",
                    "admin@admin.com",
                    "$2a$11$BUXySD8qQ2D0HSrmxRAVqOGp.QFEtYTrEczH85V.YgQc356qk9K7e",
                    null,
                    new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    null,
                    true
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[]
                {
                    AdminRoleId,
                    "ADMIN",
                    AdminUserId
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: AdminRoleId);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: AdminUserId);
        }
    }
}
