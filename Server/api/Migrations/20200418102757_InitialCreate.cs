using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEASUREMENT",
                columns: table => new
                {
                    MEASUREMENT_ID = table.Column<Guid>(nullable: false),
                    FIRE_SENSOR = table.Column<double>(nullable: false),
                    CONTENT_SENSOR = table.Column<double>(nullable: false),
                    SENSOR_1 = table.Column<double>(nullable: false),
                    SENSOR_2 = table.Column<double>(nullable: false),
                    SENSOR_3 = table.Column<double>(nullable: false),
                    SENSOR_4 = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEASUREMENT", x => x.MEASUREMENT_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEASUREMENT");
        }
    }
}
