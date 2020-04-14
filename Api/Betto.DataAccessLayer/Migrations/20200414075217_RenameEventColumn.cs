using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class RenameEventColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("HiddenTicketResult", "Events", "HiddenEventResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("HiddenEventResult", "Events", "HiddenTicketResult");
        }
    }
}
