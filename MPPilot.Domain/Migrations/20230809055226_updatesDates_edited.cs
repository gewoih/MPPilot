using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class updatesDates_edited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(8), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9917), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(351), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(239), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9739), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9636), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9371), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9167), new TimeSpan(0, 7, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(8), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9917), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(351), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 383, DateTimeKind.Unspecified).AddTicks(239), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9739), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9636), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9371), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 12, 20, 50, 382, DateTimeKind.Unspecified).AddTicks(9167), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
