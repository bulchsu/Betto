using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class RenameTicketEntityColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("PendingDateTime", "Tickets", "RevealDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("RevealDateTime", "Tickets", "PendingDateTime");
        }
    }
}
