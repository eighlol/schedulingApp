using Microsoft.EntityFrameworkCore.Migrations;

namespace SchedulingApp.Migrations
{
    public partial class MemberIsAttachedToTheUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Members");
        }
    }
}
