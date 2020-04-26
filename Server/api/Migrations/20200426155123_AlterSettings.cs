using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AlterSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SENOR_ID",
                table: "ALERT",
                newName: "SENSOR_ID");

            migrationBuilder.AddColumn<DateTime>(
                name: "LAST_SETTINGS_ACTIVATION",
                table: "SETTING",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LAST_SETTINGS_ACTIVATION",
                table: "SETTING");

            migrationBuilder.RenameColumn(
                name: "SENSOR_ID",
                table: "ALERT",
                newName: "SENOR_ID");
        }
    }
}
