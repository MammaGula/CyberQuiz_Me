using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ImproveDatabaseEfficiency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserResult_UserId_QuestionId_Id",
                table: "UserResults",
                columns: new[] { "UserId", "QuestionId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserResult_UserId_QuestionId_Id",
                table: "UserResults");
        }
    }
}
