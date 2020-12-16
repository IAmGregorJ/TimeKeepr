using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeepr.EntityFramework.Migrations
{
    public partial class addUsername2EventCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EventCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EventCategories");
        }
    }
}
