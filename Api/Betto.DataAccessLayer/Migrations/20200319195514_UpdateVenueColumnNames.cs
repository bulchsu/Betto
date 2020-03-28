using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class UpdateVenueColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenueAddress",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "VenueCapacity",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "VenueCity",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "VenueName",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "VenueSurface",
                table: "Venues");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Venues",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Venues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Venues",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Venues",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surface",
                table: "Venues",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "Surface",
                table: "Venues");

            migrationBuilder.AddColumn<string>(
                name: "VenueAddress",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VenueCapacity",
                table: "Venues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VenueCity",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VenueName",
                table: "Venues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VenueSurface",
                table: "Venues",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
