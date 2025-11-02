using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalWebApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedHiddenCarProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Available",
                table: "Cars",
                newName: "IsVisible");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Cars",
                newName: "Available");
        }
    }
}
