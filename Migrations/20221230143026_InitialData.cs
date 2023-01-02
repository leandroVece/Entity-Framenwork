using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatziEmtityF.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Description", "Name", "Salubrity" },
                values: new object[,]
                {
                    { new Guid("0a9fa564-0604-4dfa-88df-3636fe395651"), null, "Actividad recreativa", "sadudable y recomendable" },
                    { new Guid("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"), null, "Asunto domesticos", "Insalubre" }
                });

            migrationBuilder.InsertData(
                table: "TasK",
                columns: new[] { "Id", "Date", "Description", "IdCategory", "PriorityTask", "Title" },
                values: new object[,]
                {
                    { new Guid("629f9587-abc8-4c85-859f-acb762b754ed"), new DateTime(2022, 12, 30, 11, 30, 26, 753, DateTimeKind.Local).AddTicks(6157), null, new Guid("0a9fa564-0604-4dfa-88df-3636fe395651"), "medium", "Practica con el arco" },
                    { new Guid("f5d327bf-be98-4786-81d5-0a2412b7807e"), new DateTime(2022, 12, 30, 11, 30, 26, 753, DateTimeKind.Local).AddTicks(6141), null, new Guid("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"), "medium", "Limpiar Baño" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TasK",
                keyColumn: "Id",
                keyValue: new Guid("629f9587-abc8-4c85-859f-acb762b754ed"));

            migrationBuilder.DeleteData(
                table: "TasK",
                keyColumn: "Id",
                keyValue: new Guid("f5d327bf-be98-4786-81d5-0a2412b7807e"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("0a9fa564-0604-4dfa-88df-3636fe395651"));

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"));
        }
    }
}
