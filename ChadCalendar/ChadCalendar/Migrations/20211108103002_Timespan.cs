using Microsoft.EntityFrameworkCore.Migrations;

namespace ChadCalendar.Migrations
{
    public partial class Timespan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HoursTakes",
                table: "Tasks",
                newName: "TimeTakes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeTakes",
                table: "Tasks",
                newName: "HoursTakes");
        }
    }
}
