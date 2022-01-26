using Microsoft.EntityFrameworkCore.Migrations;

namespace IMWebAPI.Migrations
{
    public partial class Add_Axes_to_Reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Axis1Name",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Axis2Name",
                table: "Reports",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Axis1Name",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Axis2Name",
                table: "Reports");
        }
    }
}
