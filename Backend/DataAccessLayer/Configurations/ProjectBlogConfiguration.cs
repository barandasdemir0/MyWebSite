using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class ProjectBlogConfiguration : IEntityTypeConfiguration<ProjectBlog>
    {
        public void Configure(EntityTypeBuilder<ProjectBlog> builder)
        {
            builder.HasKey(pb => new { pb.ProjectId, pb.BlogPostId });

            builder.HasOne(pb => pb.Project).WithMany(p => p.ProjectBlogs).HasForeignKey(pb => pb.ProjectId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pb => pb.BlogPost).WithMany(p => p.ProjectBlogs).HasForeignKey(pb => pb.BlogPostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
