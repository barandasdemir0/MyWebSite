using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameChangedJobSkillPercentageAndTableNameChangedJobSkillCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobSkillPercentange",
                table: "JobSkill",
                newName: "JobSkillPercentage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobSkillPercentage",
                table: "JobSkill",
                newName: "JobSkillPercentange");
        }
    }
}
