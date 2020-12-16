using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeepr.EntityFramework.Migrations
{
    public partial class addYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Happenings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Happenings");
        }
    }
}
