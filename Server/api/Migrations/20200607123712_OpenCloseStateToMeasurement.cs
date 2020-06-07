using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class OpenCloseStateToMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OPEN_CLOSE_STATE",
                table: "MEASUREMENT",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OPEN_CLOSE_STATE",
                table: "MEASUREMENT");
        }
    }
}
