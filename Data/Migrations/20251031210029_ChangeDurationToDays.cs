using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalWebApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDurationToDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Rentals");

            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "Rentals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "Rentals");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Rentals",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
