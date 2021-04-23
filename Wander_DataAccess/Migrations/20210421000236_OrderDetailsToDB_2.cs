using Microsoft.EntityFrameworkCore.Migrations;

namespace Wander_DataAccess.Migrations
{
    public partial class OrderDetailsToDB_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Property_PropertyId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Property_PropertyId",
                table: "OrderDetails",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Property_PropertyId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Property_PropertyId",
                table: "OrderDetails",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
