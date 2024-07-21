using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Context.Migrations
{
    /// <inheritdoc />
    public partial class addlanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ar_Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ar_ModelName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ar_Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Ar_ModelName",
                table: "Products");
        }
    }
}
