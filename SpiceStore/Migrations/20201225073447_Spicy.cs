using Microsoft.EntityFrameworkCore.Migrations;

namespace SpiceStore.Migrations
{
    public partial class Spicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "spicy",
                table: "MenuItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "spicy",
                table: "MenuItem");
        }
    }
}
