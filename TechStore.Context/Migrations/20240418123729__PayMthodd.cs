using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Context.Migrations
{
    /// <inheritdoc />
    public partial class _PayMthodd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayMethod",
                table: "Orders");
        }
    }
}
