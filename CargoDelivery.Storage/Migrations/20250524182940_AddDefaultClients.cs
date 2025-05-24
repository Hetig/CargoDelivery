using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CargoDelivery.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6b0dd6ef-fa49-4b24-8471-b7b74a10c5e6"), "Client3" },
                    { new Guid("7c9a412f-409e-47e1-b2c0-93961f6f4853"), "Client1" },
                    { new Guid("c9cd28cc-b080-4509-96c8-145049f7908c"), "Client2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("6b0dd6ef-fa49-4b24-8471-b7b74a10c5e6"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("7c9a412f-409e-47e1-b2c0-93961f6f4853"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("c9cd28cc-b080-4509-96c8-145049f7908c"));
        }
    }
}
