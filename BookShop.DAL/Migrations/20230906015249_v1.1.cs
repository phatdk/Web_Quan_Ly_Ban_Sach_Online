using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.DAL.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBooks_Books_Id_BookDetail",
                table: "ProductBooks");

            migrationBuilder.RenameColumn(
                name: "Id_BookDetail",
                table: "ProductBooks",
                newName: "Id_Book");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBooks_Id_BookDetail",
                table: "ProductBooks",
                newName: "IX_ProductBooks_Id_Book");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBooks_Books_Id_Book",
                table: "ProductBooks",
                column: "Id_Book",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBooks_Books_Id_Book",
                table: "ProductBooks");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Id_Book",
                table: "ProductBooks",
                newName: "Id_BookDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBooks_Id_Book",
                table: "ProductBooks",
                newName: "IX_ProductBooks_Id_BookDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBooks_Books_Id_BookDetail",
                table: "ProductBooks",
                column: "Id_BookDetail",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
