using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.DAL.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluates_OrderDetails_Id_Book",
                table: "Evaluates");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Promotions_Id_Promotion",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_StatusOrders_Id_Promotion",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransactionsHistories_Orders_Id_Parents",
                table: "PointTransactionsHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransactionsHistories_Promotions_Id_Parents",
                table: "PointTransactionsHistories");

            migrationBuilder.DropIndex(
                name: "IX_PointTransactionsHistories_Id_Parents",
                table: "PointTransactionsHistories");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Id_Promotion",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id_Parents",
                table: "PointTransactionsHistories");

            migrationBuilder.DropColumn(
                name: "Id_Promotion",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Id_Book",
                table: "Evaluates",
                newName: "Id_Product");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluates_Id_Book",
                table: "Evaluates",
                newName: "IX_Evaluates_Id_Product");

            migrationBuilder.AddColumn<int>(
                name: "Id_Order",
                table: "PointTransactionsHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_Promotion",
                table: "PointTransactionsHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Books",
                type: "nvarchar(13)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrderPromotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Order = table.Column<int>(type: "int", nullable: false),
                    Id_Promotion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPromotions_Orders_Id_Order",
                        column: x => x.Id_Order,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPromotions_Promotions_Id_Promotion",
                        column: x => x.Id_Promotion,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactionsHistories_Id_Order",
                table: "PointTransactionsHistories",
                column: "Id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactionsHistories_Id_Promotion",
                table: "PointTransactionsHistories",
                column: "Id_Promotion");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id_StatusOrder",
                table: "Orders",
                column: "Id_StatusOrder");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPromotions_Id_Order",
                table: "OrderPromotions",
                column: "Id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPromotions_Id_Promotion",
                table: "OrderPromotions",
                column: "Id_Promotion");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluates_OrderDetails_Id_Product",
                table: "Evaluates",
                column: "Id_Product",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_StatusOrders_Id_StatusOrder",
                table: "Orders",
                column: "Id_StatusOrder",
                principalTable: "StatusOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransactionsHistories_Orders_Id_Order",
                table: "PointTransactionsHistories",
                column: "Id_Order",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransactionsHistories_Promotions_Id_Promotion",
                table: "PointTransactionsHistories",
                column: "Id_Promotion",
                principalTable: "Promotions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluates_OrderDetails_Id_Product",
                table: "Evaluates");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_StatusOrders_Id_StatusOrder",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransactionsHistories_Orders_Id_Order",
                table: "PointTransactionsHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransactionsHistories_Promotions_Id_Promotion",
                table: "PointTransactionsHistories");

            migrationBuilder.DropTable(
                name: "OrderPromotions");

            migrationBuilder.DropIndex(
                name: "IX_PointTransactionsHistories_Id_Order",
                table: "PointTransactionsHistories");

            migrationBuilder.DropIndex(
                name: "IX_PointTransactionsHistories_Id_Promotion",
                table: "PointTransactionsHistories");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Id_StatusOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id_Order",
                table: "PointTransactionsHistories");

            migrationBuilder.DropColumn(
                name: "Id_Promotion",
                table: "PointTransactionsHistories");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Id_Product",
                table: "Evaluates",
                newName: "Id_Book");

            migrationBuilder.RenameIndex(
                name: "IX_Evaluates_Id_Product",
                table: "Evaluates",
                newName: "IX_Evaluates_Id_Book");

            migrationBuilder.AddColumn<int>(
                name: "Id_Parents",
                table: "PointTransactionsHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_Promotion",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactionsHistories_Id_Parents",
                table: "PointTransactionsHistories",
                column: "Id_Parents");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id_Promotion",
                table: "Orders",
                column: "Id_Promotion");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluates_OrderDetails_Id_Book",
                table: "Evaluates",
                column: "Id_Book",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Promotions_Id_Promotion",
                table: "Orders",
                column: "Id_Promotion",
                principalTable: "Promotions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_StatusOrders_Id_Promotion",
                table: "Orders",
                column: "Id_Promotion",
                principalTable: "StatusOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransactionsHistories_Orders_Id_Parents",
                table: "PointTransactionsHistories",
                column: "Id_Parents",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransactionsHistories_Promotions_Id_Parents",
                table: "PointTransactionsHistories",
                column: "Id_Parents",
                principalTable: "Promotions",
                principalColumn: "Id");
        }
    }
}
