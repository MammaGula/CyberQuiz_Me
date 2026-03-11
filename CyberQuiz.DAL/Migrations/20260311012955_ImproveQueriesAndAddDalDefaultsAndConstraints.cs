using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberQuiz.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ImproveQueriesAndAddDalDefaultsAndConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "UserResults",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories",
                columns: new[] { "CategoryId", "SortOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId",
                unique: true,
                filter: "[IsCorrect] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions",
                columns: new[] { "QuestionId", "DisplayOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "UserResults",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<int>(
                name: "Points",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId_SortOrder",
                table: "SubCategories",
                columns: new[] { "CategoryId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId_DisplayOrder",
                table: "AnswerOptions",
                columns: new[] { "QuestionId", "DisplayOrder" });
        }
    }
}
