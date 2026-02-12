using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddStickerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sticker",
                table: "Devices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewSticker",
                table: "DeviceMovements",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldSticker",
                table: "DeviceMovements",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Sticker",
                table: "Devices",
                column: "Sticker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_Sticker",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Sticker",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "NewSticker",
                table: "DeviceMovements");

            migrationBuilder.DropColumn(
                name: "OldSticker",
                table: "DeviceMovements");
        }
    }
}
