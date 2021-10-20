using Microsoft.EntityFrameworkCore.Migrations;

namespace ChadCalendar.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    WorkingHoursFrom = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkingHoursTo = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeZone = table.Column<int>(type: "INTEGER", nullable: false),
                    RemindEveryNDays = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
