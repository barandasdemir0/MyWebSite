using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class viewcountDeletedBlogPostAndAddingProjectpublished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "IsFeatured",
                table: "Projects",
                newName: "IsPublished");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Projects",
                newName: "IsFeatured");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
