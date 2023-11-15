using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace checkinItTwice_BE.Migrations
{
    public partial class listAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearId",
                table: "ChristmasLists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearId",
                table: "ChristmasLists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
