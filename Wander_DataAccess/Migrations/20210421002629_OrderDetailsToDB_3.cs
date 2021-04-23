using Microsoft.EntityFrameworkCore.Migrations;

namespace Wander_DataAccess.Migrations
{
    public partial class OrderDetailsToDB_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrderDetails");
        }
    }
}
