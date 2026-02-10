using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ForSkillThings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillIcon",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "SkillUrl",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "VisualType",
                table: "Skills",
                newName: "IconifyIcon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IconifyIcon",
                table: "Skills",
                newName: "VisualType");

            migrationBuilder.AddColumn<string>(
                name: "SkillIcon",
                table: "Skills",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SkillUrl",
                table: "Skills",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
