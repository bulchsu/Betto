using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class AddHiddenResultToTicketEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HiddenTicketResult",
                table: "Events",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiddenTicketResult",
                table: "Events");
        }
    }
}
