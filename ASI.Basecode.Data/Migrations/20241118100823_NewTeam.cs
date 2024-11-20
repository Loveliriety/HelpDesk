using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    public partial class NewTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamLeader",
                table: "Teams",
                newName: "Tier");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "Teams",
                newName: "TeamLeader");
        }
    }
}
