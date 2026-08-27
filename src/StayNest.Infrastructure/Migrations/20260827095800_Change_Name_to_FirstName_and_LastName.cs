using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_Name_to_FirstName_and_LastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "lastname");

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstname",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "firstname",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "users",
                newName: "name");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
