using Microsoft.EntityFrameworkCore.Migrations;

namespace TestAPI.Migrations
{
    public partial class migrationv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Order__5629CD9C",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Produ__571DF1D5",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PurchaseO__Custo__4F7CD00D",
                table: "PurchaseOrder");

            migrationBuilder.DropForeignKey(
                name: "FK__PurchaseO__Emplo__5070F446",
                table: "PurchaseOrder");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Order__5629CD9C",
                table: "OrderDetail",
                column: "OrderID",
                principalTable: "PurchaseOrder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Produ__571DF1D5",
                table: "OrderDetail",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__PurchaseO__Custo__4F7CD00D",
                table: "PurchaseOrder",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__PurchaseO__Emplo__5070F446",
                table: "PurchaseOrder",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Order__5629CD9C",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__OrderDeta__Produ__571DF1D5",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK__PurchaseO__Custo__4F7CD00D",
                table: "PurchaseOrder");

            migrationBuilder.DropForeignKey(
                name: "FK__PurchaseO__Emplo__5070F446",
                table: "PurchaseOrder");

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Order__5629CD9C",
                table: "OrderDetail",
                column: "OrderID",
                principalTable: "PurchaseOrder",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__OrderDeta__Produ__571DF1D5",
                table: "OrderDetail",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__PurchaseO__Custo__4F7CD00D",
                table: "PurchaseOrder",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__PurchaseO__Emplo__5070F446",
                table: "PurchaseOrder",
                column: "EmployeeID",
                principalTable: "Employee",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
