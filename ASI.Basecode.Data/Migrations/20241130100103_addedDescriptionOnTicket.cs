using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    public partial class addedDescriptionOnTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tickets");
        }
    }
}
