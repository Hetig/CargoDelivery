using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoDelivery.Storage.Migrations
{
    /// <inheritdoc />
    public partial class ManyToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cargos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cargos");
        }
    }
}
