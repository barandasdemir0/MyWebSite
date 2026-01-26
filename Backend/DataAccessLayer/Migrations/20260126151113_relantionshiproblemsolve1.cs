using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class relantionshiproblemsolve1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTopics_BlogPosts_TopicId",
                table: "BlogTopics");

            migrationBuilder.DropTable(
                name: "ProjectBlogs");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTopics_BlogPosts_BlogPostId",
                table: "BlogTopics",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTopics_BlogPosts_BlogPostId",
                table: "BlogTopics");

            migrationBuilder.CreateTable(
                name: "ProjectBlogs",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBlogs", x => new { x.ProjectId, x.BlogPostId });
                    table.ForeignKey(
                        name: "FK_ProjectBlogs_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectBlogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBlogs_BlogPostId",
                table: "ProjectBlogs",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTopics_BlogPosts_TopicId",
                table: "BlogTopics",
                column: "TopicId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
