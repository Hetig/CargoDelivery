using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CargoDelivery.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultCouriers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8a9df209-331b-417b-87eb-ff2c2762b47c"), "Courier2" },
                    { new Guid("9bab18df-c059-414b-9e8c-7e3825c9eb28"), "Courier1" },
                    { new Guid("deaedbb9-8e88-44a4-a639-8d6f5c43ca5f"), "Courier3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Couriers",
                keyColumn: "Id",
                keyValue: new Guid("8a9df209-331b-417b-87eb-ff2c2762b47c"));

            migrationBuilder.DeleteData(
                table: "Couriers",
                keyColumn: "Id",
                keyValue: new Guid("9bab18df-c059-414b-9e8c-7e3825c9eb28"));

            migrationBuilder.DeleteData(
                table: "Couriers",
                keyColumn: "Id",
                keyValue: new Guid("deaedbb9-8e88-44a4-a639-8d6f5c43ca5f"));
        }
    }
}
