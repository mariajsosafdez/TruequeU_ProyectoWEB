using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruequeU.Migrations
{
    /// <inheritdoc />
    public partial class FixIsActiveListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Listings");
        }
    }
}
