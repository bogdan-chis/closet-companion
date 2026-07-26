using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClosetCompanionApp.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceOutfitGarmentJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedGarmentIds",
                table: "GeneratedOutfits");

            migrationBuilder.CreateTable(
                name: "OutfitGarments",
                columns: table => new
                {
                    GeneratedOutfitId = table.Column<Guid>(type: "uuid", nullable: false),
                    GarmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutfitGarments", x => new { x.GeneratedOutfitId, x.GarmentId });
                    table.ForeignKey(
                        name: "FK_OutfitGarments_Garments_GarmentId",
                        column: x => x.GarmentId,
                        principalTable: "Garments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutfitGarments_GeneratedOutfits_GeneratedOutfitId",
                        column: x => x.GeneratedOutfitId,
                        principalTable: "GeneratedOutfits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutfitGarments_GarmentId",
                table: "OutfitGarments",
                column: "GarmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutfitGarments");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "SelectedGarmentIds",
                table: "GeneratedOutfits",
                type: "uuid[]",
                nullable: false);
        }
    }
}
