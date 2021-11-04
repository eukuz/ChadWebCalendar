using Microsoft.EntityFrameworkCore.Migrations;

namespace ChadCalendar.Migrations
{
    public partial class SuccessorRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_PredecessorFK",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SuccessorFK",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PredecessorFK",
                table: "Tasks",
                column: "PredecessorFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_PredecessorFK",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "SuccessorFK",
                table: "Tasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PredecessorFK",
                table: "Tasks",
                column: "PredecessorFK",
                unique: true);
        }
    }
}
