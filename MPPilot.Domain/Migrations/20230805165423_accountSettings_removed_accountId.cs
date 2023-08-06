using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.App.Migrations
{
    /// <inheritdoc />
    public partial class accountSettings_removed_accountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountSettings_Accounts_AccountId",
                table: "AccountSettings");

            migrationBuilder.DropIndex(
                name: "IX_AccountSettings_AccountId",
                table: "AccountSettings");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "AccountSettings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 23, 54, 23, 342, DateTimeKind.Unspecified).AddTicks(8150), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 20, 33, 44, 54, DateTimeKind.Unspecified).AddTicks(2019), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 23, 54, 23, 342, DateTimeKind.Unspecified).AddTicks(7922), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 20, 33, 44, 54, DateTimeKind.Unspecified).AddTicks(1801), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountSettingsId",
                table: "Accounts",
                column: "AccountSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountSettings_AccountSettingsId",
                table: "Accounts",
                column: "AccountSettingsId",
                principalTable: "AccountSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountSettings_AccountSettingsId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountSettingsId",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "AccountSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 20, 33, 44, 54, DateTimeKind.Unspecified).AddTicks(2019), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 23, 54, 23, 342, DateTimeKind.Unspecified).AddTicks(8150), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 20, 33, 44, 54, DateTimeKind.Unspecified).AddTicks(1801), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 5, 23, 54, 23, 342, DateTimeKind.Unspecified).AddTicks(7922), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_AccountSettings_AccountId",
                table: "AccountSettings",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountSettings_Accounts_AccountId",
                table: "AccountSettings",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
