using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.DAL.Migrations
{
    public partial class v131 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReduceMax",
                table: "UserPromotions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReduceMax",
                table: "UserPromotions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
