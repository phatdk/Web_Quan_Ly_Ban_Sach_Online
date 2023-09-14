using System;
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
                name: "Value",
                table: "PropertyValues",
                newName: "Value1");

            migrationBuilder.RenameColumn(
                name: "Id_BookDetail",
                table: "ProductBooks",
                newName: "Id_Book");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBooks_Id_BookDetail",
                table: "ProductBooks",
                newName: "IX_ProductBooks_Id_Book");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "PropertyValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PercentReduct",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Condition",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AmountReduct",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StorageTerm",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreatedDate", "Email", "Name", "Password", "Phone", "Role", "Status" },
                values: new object[] { 1, new DateTime(2023, 9, 13, 15, 32, 35, 434, DateTimeKind.Local).AddTicks(9291), "example@gmail.com", "Admin", "1", "0000000000", 0, 1 });

            migrationBuilder.InsertData(
                table: "PromotionTypes",
                columns: new[] { "Id", "CreatedDate", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khuyến mại theo đơn", 0 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khuyến mại theo sản phẩm", 0 },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khuyến mại đổi điểm", 0 }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "About", "ShopName" },
                values: new object[] { 1, "Một số thông tin về shop", "Wild Rose" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birth", "CreatedDate", "Email", "Gender", "Name", "Password", "Phone", "Status", "UserName" },
                values: new object[] { 1, null, new DateTime(2023, 9, 13, 15, 32, 35, 434, DateTimeKind.Local).AddTicks(9482), null, null, "Khách vẵng lai", "1", null, 1, "customer" });

            migrationBuilder.InsertData(
                table: "CustomProperties",
                columns: new[] { "Id", "Id_Shop", "propertyName" },
                values: new object[] { 1, 1, "Logo" });

            migrationBuilder.InsertData(
                table: "CustomProperties",
                columns: new[] { "Id", "Id_Shop", "propertyName" },
                values: new object[] { 2, 1, "Banner" });

            migrationBuilder.InsertData(
                table: "CustomProperties",
                columns: new[] { "Id", "Id_Shop", "propertyName" },
                values: new object[] { 3, 1, "Event banner" });

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

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomProperties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomProperties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CustomProperties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PromotionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PromotionTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PromotionTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Img",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "PropertyValues");

            migrationBuilder.DropColumn(
                name: "StorageTerm",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Value1",
                table: "PropertyValues",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Id_Book",
                table: "ProductBooks",
                newName: "Id_BookDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBooks_Id_Book",
                table: "ProductBooks",
                newName: "IX_ProductBooks_Id_BookDetail");

            migrationBuilder.AlterColumn<int>(
                name: "PercentReduct",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Condition",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AmountReduct",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
