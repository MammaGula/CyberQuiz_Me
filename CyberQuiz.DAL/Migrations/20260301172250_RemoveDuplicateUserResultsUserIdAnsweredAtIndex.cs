using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateUserResultsUserIdAnsweredAtIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserResults_UserId_AnsweredAt",
                table: "UserResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserResults_UserId_AnsweredAt",
                table: "UserResults",
                columns: new[] { "UserId", "AnsweredAt" });
        }
    }
}
