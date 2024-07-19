using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leaderboard.Infrastructure.Migrations
{
    public partial class AddWhoHasEnteredThePoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedByUserId",
                table: "Points",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Points_AddedByUserId",
                table: "Points",
                column: "AddedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_AspNetUsers_AddedByUserId",
                table: "Points",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_AspNetUsers_AddedByUserId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_AddedByUserId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "AddedByUserId",
                table: "Points");
        }
    }
}
