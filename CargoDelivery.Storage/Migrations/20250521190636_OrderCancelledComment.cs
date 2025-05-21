using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDelivery.Storage.Migrations
{
    /// <inheritdoc />
    public partial class OrderCancelledComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedComment",
                table: "Orders",
                newName: "CancelledComment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CancelledComment",
                table: "Orders",
                newName: "DeletedComment");
        }
    }
}
