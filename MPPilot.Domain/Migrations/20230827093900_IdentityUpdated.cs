using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autobidders_AspNetUsers_AccountId",
                table: "Autobidders");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Autobidders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Autobidders_AccountId",
                table: "Autobidders",
                newName: "IX_Autobidders_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Autobidders_AspNetUsers_UserId",
                table: "Autobidders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Autobidders_AspNetUsers_UserId",
                table: "Autobidders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Autobidders",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Autobidders_UserId",
                table: "Autobidders",
                newName: "IX_Autobidders_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Autobidders_AspNetUsers_AccountId",
                table: "Autobidders",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
