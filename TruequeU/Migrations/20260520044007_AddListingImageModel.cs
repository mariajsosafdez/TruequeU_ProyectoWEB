using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruequeU.Migrations
{
    /// <inheritdoc />
    public partial class AddListingImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingImages",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_ListingImages_Listings_ListingID",
                        column: x => x.ListingID,
                        principalTable: "Listings",
                        principalColumn: "IdListing");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingID",
                table: "ListingImages",
                column: "ListingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingImages");
        }
    }
}
