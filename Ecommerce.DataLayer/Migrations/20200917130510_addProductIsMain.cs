using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecommerce.DataLayer.Migrations
{
    public partial class addProductIsMain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Products");
        }
    }
}
