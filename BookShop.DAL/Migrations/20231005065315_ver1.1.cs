using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.DAL.Migrations
{
    public partial class ver11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins");

            migrationBuilder.AddColumn<int>(
                name: "UserrId",
                table: "UserTokens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserrId",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserrId",
                table: "UserLogins",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id_Collection",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserrId",
                table: "UserTokens",
                column: "UserrId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserrId",
                table: "UserRoles",
                column: "UserrId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserrId",
                table: "UserLogins",
                column: "UserrId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Users_UserrId",
                table: "UserLogins",
                column: "UserrId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserrId",
                table: "UserRoles",
                column: "UserrId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserrId",
                table: "UserTokens",
                column: "UserrId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Users_UserrId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserrId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserrId",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserrId",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserrId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_UserrId",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "UserrId",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "UserrId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserrId",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "Id_Collection",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
