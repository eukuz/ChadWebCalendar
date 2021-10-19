using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChadCalendar.Migrations
{
    public partial class TaskFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
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
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Duty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Accessed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NRepetitions = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<int>(type: "INTEGER", nullable: false),
                    Frequency = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IconNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: true),
                    HoursTakes = table.Column<decimal>(type: "TEXT", nullable: true),
                    AllowedToDistribute = table.Column<bool>(type: "INTEGER", nullable: true),
                    MaxPerDay = table.Column<int>(type: "INTEGER", nullable: true),
                    Task_Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PredecessorFK = table.Column<int>(type: "INTEGER", nullable: true),
                    SuccessorFK = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duty_Duty_PredecessorFK",
                        column: x => x.PredecessorFK,
                        principalTable: "Duty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Duty_Duty_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Duty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Duty_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duty_PredecessorFK",
                table: "Duty",
                column: "PredecessorFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Duty_ProjectId",
                table: "Duty",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Duty_UserId",
                table: "Duty",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duty");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
