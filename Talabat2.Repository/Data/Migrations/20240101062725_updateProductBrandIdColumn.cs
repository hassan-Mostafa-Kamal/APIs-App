using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat2.Repository.Data.Migrations
{
    public partial class updateProductBrandIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodects_productBrands_ProdectBrandId",
                table: "Prodects");

            migrationBuilder.RenameColumn(
                name: "ProdectBrandId",
                table: "Prodects",
                newName: "ProductBrandId");

            migrationBuilder.RenameIndex(
                name: "IX_Prodects_ProdectBrandId",
                table: "Prodects",
                newName: "IX_Prodects_ProductBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodects_productBrands_ProductBrandId",
                table: "Prodects",
                column: "ProductBrandId",
                principalTable: "productBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodects_productBrands_ProductBrandId",
                table: "Prodects");

            migrationBuilder.RenameColumn(
                name: "ProductBrandId",
                table: "Prodects",
                newName: "ProdectBrandId");

            migrationBuilder.RenameIndex(
                name: "IX_Prodects_ProductBrandId",
                table: "Prodects",
                newName: "IX_Prodects_ProdectBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodects_productBrands_ProdectBrandId",
                table: "Prodects",
                column: "ProdectBrandId",
                principalTable: "productBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
