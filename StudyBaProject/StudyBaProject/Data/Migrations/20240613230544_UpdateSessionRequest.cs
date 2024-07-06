using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBaProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSessionRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "SessionRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "SessionRequest");
        }
    }
}
