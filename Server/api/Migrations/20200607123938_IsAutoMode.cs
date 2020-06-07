using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class IsAutoMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_AUTO_MODE",
                table: "MEASUREMENT",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_AUTO_MODE",
                table: "MEASUREMENT");
        }
    }
}
