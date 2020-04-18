using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class TimestampsToMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TIME_STAMP_SMOKER",
                table: "MEASUREMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TIME_STAMP_RECEIVED",
                table: "MEASUREMENT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TIME_STAMP_SMOKER",
                table: "MEASUREMENT");

            migrationBuilder.DropColumn(
                name: "TIME_STAMP_RECEIVED",
                table: "MEASUREMENT");
        }
    }
}
