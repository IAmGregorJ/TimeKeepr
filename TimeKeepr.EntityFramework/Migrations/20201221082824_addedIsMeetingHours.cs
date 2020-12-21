using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeKeepr.EntityFramework.Migrations
{
    public partial class addedIsMeetingHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "IsMeetingHours",
                table: "Happenings",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMeetingHours",
                table: "Happenings");
        }
    }
}
