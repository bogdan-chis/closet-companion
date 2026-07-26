using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClosetCompanionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPosePhotoCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GeneratedOutfits_BasePhotoId",
                table: "GeneratedOutfits",
                column: "BasePhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneratedOutfits_PosePhoto_BasePhotoId",
                table: "GeneratedOutfits",
                column: "BasePhotoId",
                principalTable: "PosePhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneratedOutfits_PosePhoto_BasePhotoId",
                table: "GeneratedOutfits");

            migrationBuilder.DropIndex(
                name: "IX_GeneratedOutfits_BasePhotoId",
                table: "GeneratedOutfits");
        }
    }
}
