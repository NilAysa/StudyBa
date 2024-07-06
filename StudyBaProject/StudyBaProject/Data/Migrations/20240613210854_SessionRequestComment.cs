using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBaProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class SessionRequestComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "SessionRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "SessionRequest");
        }
    }
}
