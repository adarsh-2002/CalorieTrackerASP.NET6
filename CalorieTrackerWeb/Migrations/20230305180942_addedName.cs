using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieTrackerWeb.Migrations
{
    /// <inheritdoc />
    public partial class addedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FoodEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FoodEntries");
        }
    }
}
