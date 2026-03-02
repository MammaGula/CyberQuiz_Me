using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CheckModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop index only if it exists (some deployments may have already removed it).
            migrationBuilder.Sql(@"
IF EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_UserResults_UserId_AnsweredAt' AND object_id = OBJECT_ID('dbo.UserResults'))
BEGIN
    DROP INDEX IX_UserResults_UserId_AnsweredAt ON dbo.UserResults;
END");
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
