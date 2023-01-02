using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatziEmtityF.Migrations
{
    /// <inheritdoc />
    public partial class ColumnSalubrityCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salubrity",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salubrity",
                table: "Category");
        }
    }
}
