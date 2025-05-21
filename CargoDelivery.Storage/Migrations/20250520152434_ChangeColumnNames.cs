using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDelivery.Storage.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cargos_CargoDbId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientDbId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Couriers_CourierDbId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CourierDbId",
                table: "Orders",
                newName: "CourierId");

            migrationBuilder.RenameColumn(
                name: "ClientDbId",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CargoDbId",
                table: "Orders",
                newName: "CargoId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CourierDbId",
                table: "Orders",
                newName: "IX_Orders_CourierId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientDbId",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CargoDbId",
                table: "Orders",
                newName: "IX_Orders_CargoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cargos_CargoId",
                table: "Orders",
                column: "CargoId",
                principalTable: "Cargos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Couriers_CourierId",
                table: "Orders",
                column: "CourierId",
                principalTable: "Couriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cargos_CargoId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Couriers_CourierId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CourierId",
                table: "Orders",
                newName: "CourierDbId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "ClientDbId");

            migrationBuilder.RenameColumn(
                name: "CargoId",
                table: "Orders",
                newName: "CargoDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CourierId",
                table: "Orders",
                newName: "IX_Orders_CourierDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                newName: "IX_Orders_ClientDbId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CargoId",
                table: "Orders",
                newName: "IX_Orders_CargoDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cargos_CargoDbId",
                table: "Orders",
                column: "CargoDbId",
                principalTable: "Cargos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientDbId",
                table: "Orders",
                column: "ClientDbId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Couriers_CourierDbId",
                table: "Orders",
                column: "CourierDbId",
                principalTable: "Couriers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
