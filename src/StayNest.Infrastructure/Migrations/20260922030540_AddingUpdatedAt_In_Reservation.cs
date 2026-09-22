using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingUpdatedAt_In_Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "reservations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "reservations");
        }
    }
}
