using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertBids_Autobidders_AutobidderId",
                table: "AdvertBids");

            migrationBuilder.AlterColumn<Guid>(
                name: "AutobidderId",
                table: "AdvertBids",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertBids_Autobidders_AutobidderId",
                table: "AdvertBids",
                column: "AutobidderId",
                principalTable: "Autobidders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertBids_Autobidders_AutobidderId",
                table: "AdvertBids");

            migrationBuilder.AlterColumn<Guid>(
                name: "AutobidderId",
                table: "AdvertBids",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertBids_Autobidders_AutobidderId",
                table: "AdvertBids",
                column: "AutobidderId",
                principalTable: "Autobidders",
                principalColumn: "Id");
        }
    }
}
