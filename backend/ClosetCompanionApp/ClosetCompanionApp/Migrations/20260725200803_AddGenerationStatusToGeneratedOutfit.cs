using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClosetCompanionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGenerationStatusToGeneratedOutfit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultImageUrl",
                table: "GeneratedOutfits",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "GeneratedOutfits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GeneratedOutfits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "GeneratedOutfits");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeneratedOutfits");

            migrationBuilder.AlterColumn<string>(
                name: "ResultImageUrl",
                table: "GeneratedOutfits",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
