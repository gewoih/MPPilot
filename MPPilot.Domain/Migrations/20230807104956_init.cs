using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WildberriesApiKey = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2319), new TimeSpan(0, 7, 0, 0, 0))),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2398), new TimeSpan(0, 7, 0, 0, 0))),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    AccountSettingsId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(1809), new TimeSpan(0, 7, 0, 0, 0))),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2033), new TimeSpan(0, 7, 0, 0, 0))),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountSettings_AccountSettingsId",
                        column: x => x.AccountSettingsId,
                        principalTable: "AccountSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Autobidders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AdvertId = table.Column<int>(type: "integer", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2582), new TimeSpan(0, 7, 0, 0, 0))),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2668), new TimeSpan(0, 7, 0, 0, 0))),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autobidders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autobidders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertBids",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    AutobidderMode = table.Column<int>(type: "integer", nullable: false),
                    AdvertKeyword = table.Column<string>(type: "text", nullable: false),
                    AdvertPosition = table.Column<int>(type: "integer", nullable: false),
                    TargetPositionLeftBound = table.Column<int>(type: "integer", nullable: false),
                    TargetPositionRightBound = table.Column<int>(type: "integer", nullable: false),
                    LastCPM = table.Column<int>(type: "integer", nullable: false),
                    CurrentCPM = table.Column<int>(type: "integer", nullable: false),
                    AutobidderId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2903), new TimeSpan(0, 7, 0, 0, 0))),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(3013), new TimeSpan(0, 7, 0, 0, 0))),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertBids_Autobidders_AutobidderId",
                        column: x => x.AutobidderId,
                        principalTable: "Autobidders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountSettingsId",
                table: "Accounts",
                column: "AccountSettingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountSettings_WildberriesApiKey",
                table: "AccountSettings",
                column: "WildberriesApiKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertBids_AutobidderId",
                table: "AdvertBids",
                column: "AutobidderId");

            migrationBuilder.CreateIndex(
                name: "IX_Autobidders_AccountId",
                table: "Autobidders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Autobidders_AdvertId",
                table: "Autobidders",
                column: "AdvertId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertBids");

            migrationBuilder.DropTable(
                name: "Autobidders");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountSettings");
        }
    }
}
