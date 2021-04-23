using Microsoft.EntityFrameworkCore.Migrations;

namespace Wander_DataAccess.Migrations
{
    public partial class OrderDetailsDBWithAgentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agent_Id",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agent_Id",
                table: "OrderDetails");
        }
    }
}
