using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopMVCNet7.Migrations
{
    /// <inheritdoc />
    public partial class addconfigAppOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "AppOrders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "AppOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "AppOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAddress",
                table: "AppOrders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderDetails_OrderId",
                table: "AppOrderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderDetails_AppOrders_OrderId",
                table: "AppOrderDetails",
                column: "OrderId",
                principalTable: "AppOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderDetails_AppOrders_OrderId",
                table: "AppOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_AppOrderDetails_OrderId",
                table: "AppOrderDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAddress",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
