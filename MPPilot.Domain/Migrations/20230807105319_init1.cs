using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9386), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2668), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2582), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(3013), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9629), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2903), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "WildberriesApiKey",
                table: "AccountSettings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9090), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2398), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9012), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2319), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8743), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2033), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8535), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(1809), new TimeSpan(0, 7, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2668), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9386), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2582), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(3013), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2903), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9629), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<string>(
                name: "WildberriesApiKey",
                table: "AccountSettings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2398), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9090), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2319), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9012), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(2033), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8743), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 49, 56, 393, DateTimeKind.Unspecified).AddTicks(1809), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8535), new TimeSpan(0, 7, 0, 0, 0)));
        }
    }
}
