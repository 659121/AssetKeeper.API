using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddDeviceMovementDepartmentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceMovements_Departments_FromDepartmentId",
                table: "DeviceMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceMovements_Departments_ToDepartmentId",
                table: "DeviceMovements");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceMovements_Departments_FromDepartmentId",
                table: "DeviceMovements",
                column: "FromDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceMovements_Departments_ToDepartmentId",
                table: "DeviceMovements",
                column: "ToDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceMovements_Departments_FromDepartmentId",
                table: "DeviceMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceMovements_Departments_ToDepartmentId",
                table: "DeviceMovements");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceMovements_Departments_FromDepartmentId",
                table: "DeviceMovements",
                column: "FromDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceMovements_Departments_ToDepartmentId",
                table: "DeviceMovements",
                column: "ToDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
