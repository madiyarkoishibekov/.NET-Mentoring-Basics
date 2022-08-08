using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseAccess.Migrations
{
    public partial class AddNavigationPropToOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Product_ProductModelId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProductModelId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ProductModelId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductId",
                table: "Order",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Product_ProductId",
                table: "Order",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Product_ProductId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProductId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "ProductModelId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductModelId",
                table: "Order",
                column: "ProductModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Product_ProductModelId",
                table: "Order",
                column: "ProductModelId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
