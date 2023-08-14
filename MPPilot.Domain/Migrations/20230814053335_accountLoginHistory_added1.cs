using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class accountLoginHistory_added1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginHistory_Accounts_AccountId",
                table: "LoginHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginHistory",
                table: "LoginHistory");

            migrationBuilder.RenameTable(
                name: "LoginHistory",
                newName: "LoginsHistory");

            migrationBuilder.RenameIndex(
                name: "IX_LoginHistory_AccountId",
                table: "LoginsHistory",
                newName: "IX_LoginsHistory_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginsHistory",
                table: "LoginsHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginsHistory_Accounts_AccountId",
                table: "LoginsHistory",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginsHistory_Accounts_AccountId",
                table: "LoginsHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginsHistory",
                table: "LoginsHistory");

            migrationBuilder.RenameTable(
                name: "LoginsHistory",
                newName: "LoginHistory");

            migrationBuilder.RenameIndex(
                name: "IX_LoginsHistory_AccountId",
                table: "LoginHistory",
                newName: "IX_LoginHistory_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginHistory",
                table: "LoginHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginHistory_Accounts_AccountId",
                table: "LoginHistory",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
