using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SETTING",
                columns: table => new
                {
                    SETTINGS_ID = table.Column<Guid>(nullable: false),
                    OPEN_CLOSE_TRESHOLD = table.Column<int>(nullable: false),
                    IS_AUTO_MODE = table.Column<bool>(nullable: false),
                    FIRE_NOTIFCATION_TEMPERATUR = table.Column<int>(nullable: true),
                    TEMPERATUR_UPDATE_CYCLE_SECONDS = table.Column<int>(nullable: false),
                    LAST_SETTINGS_UPDATE = table.Column<DateTime>(nullable: false),
                    LASAT_SETTINGS_UPDATE_USER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SETTING", x => x.SETTINGS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ALERT",
                columns: table => new
                {
                    ALLERT_ID = table.Column<Guid>(nullable: false),
                    SENOR_ID = table.Column<int>(nullable: false),
                    TEMPERATUR = table.Column<int>(nullable: false),
                    ALERT_TYPE = table.Column<int>(nullable: false),
                    FK_SETTING_ID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERT", x => x.ALLERT_ID);
                    table.ForeignKey(
                        name: "FK_ALERT_SETTING_FK_SETTING_ID",
                        column: x => x.FK_SETTING_ID,
                        principalTable: "SETTING",
                        principalColumn: "SETTINGS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERT_FK_SETTING_ID",
                table: "ALERT",
                column: "FK_SETTING_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERT");

            migrationBuilder.DropTable(
                name: "SETTING");
        }
    }
}
