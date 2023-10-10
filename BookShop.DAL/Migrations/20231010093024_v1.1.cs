using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.DAL.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Books",
                newName: "CoverPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                table: "Orders",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "varchar(13)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(13)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "varchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "varchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Staff",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id_Staff",
                table: "Orders",
                column: "Id_Staff");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_Id_Staff",
                table: "Orders",
                column: "Id_Staff",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_Id_Staff",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Id_Staff",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id_Staff",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CoverPrice",
                table: "Books",
                newName: "Price");

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                table: "Orders",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Orders",
                type: "varchar(13)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(13)",
                oldNullable: true);
        }
    }
}
