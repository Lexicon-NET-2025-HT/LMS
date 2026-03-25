using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infractructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackGivenBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeedbackGivenById",
                table: "Submissions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_FeedbackGivenById",
                table: "Submissions",
                column: "FeedbackGivenById");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_AspNetUsers_FeedbackGivenById",
                table: "Submissions",
                column: "FeedbackGivenById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_AspNetUsers_FeedbackGivenById",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_FeedbackGivenById",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "FeedbackGivenById",
                table: "Submissions");
        }
    }
}
