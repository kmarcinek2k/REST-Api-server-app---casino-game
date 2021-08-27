using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.Migrations
{
    public partial class BoardsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentPool",
                table: "Boards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPool",
                table: "Boards");
        }
    }
}
