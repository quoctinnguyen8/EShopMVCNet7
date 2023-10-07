using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShopMVCNet7.Migrations
{
    /// <inheritdoc />
    public partial class addFKconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts",
                column: "CategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts",
                column: "CategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
