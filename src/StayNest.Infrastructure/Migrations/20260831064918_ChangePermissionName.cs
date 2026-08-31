using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePermissionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "users:read");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "User.Read");
        }
    }
}
