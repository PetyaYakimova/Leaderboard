using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    public partial class AddPinnedContestsForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PinnedContest",
                columns: table => new
                {
                    ContestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Contest Identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinnedContest", x => new { x.ContestId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PinnedContest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PinnedContest_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Pinned Contests For Users");

            migrationBuilder.CreateIndex(
                name: "IX_PinnedContest_UserId",
                table: "PinnedContest",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PinnedContest");
        }
    }
}
