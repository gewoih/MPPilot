using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPPilot.Domain.Migrations
{
    /// <inheritdoc />
    public partial class autobidder_dailybudget_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8966), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9386), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8877), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "DailyBudget",
                table: "Autobidders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(9341), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(9218), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9629), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8704), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9090), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8622), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9012), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8361), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8743), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8133), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8535), new TimeSpan(0, 7, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyBudget",
                table: "Autobidders");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9386), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8966), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Autobidders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9277), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8877), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9719), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(9341), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AdvertBids",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9629), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(9218), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9090), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8704), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AccountSettings",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(9012), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8622), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8743), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8361), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2023, 8, 7, 17, 53, 19, 60, DateTimeKind.Unspecified).AddTicks(8535), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 8, 9, 11, 48, 54, 168, DateTimeKind.Unspecified).AddTicks(8133), new TimeSpan(0, 7, 0, 0, 0)));
        }
    }
}
