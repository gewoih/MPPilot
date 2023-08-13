using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class autobidder_bidsPausedTill_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BidsPausedTill",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidsPausedTill",
                table: "Autobidders");
        }
    }
}
