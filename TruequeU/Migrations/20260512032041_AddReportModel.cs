using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruequeU.Migrations
{
    /// <inheritdoc />
    public partial class AddReportModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    IdListing = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Condicion = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    Ubicacion = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.IdListing);
                    table.ForeignKey(
                        name: "FK_Listings_Clients_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportedListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Clients_ReportedBy",
                        column: x => x.ReportedBy,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Reports_Clients_ReportedUserId",
                        column: x => x.ReportedUserId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Reports_Listings_ReportedListingId",
                        column: x => x.ReportedListingId,
                        principalTable: "Listings",
                        principalColumn: "IdListing");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_OwnerId",
                table: "Listings",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedBy",
                table: "Reports",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedListingId",
                table: "Reports",
                column: "ReportedListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedUserId",
                table: "Reports",
                column: "ReportedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Listings");
        }
    }
}
