using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class BlogTopicConfiguration : IEntityTypeConfiguration<BlogTopic>
    {
        public void Configure(EntityTypeBuilder<BlogTopic> builder)
        {
            builder.HasKey(bt => new { bt.BlogPostId, bt.TopicId });

            builder.HasOne(bt => bt.BlogPost).WithMany(b => b.BlogTopics).HasForeignKey(bt => bt.BlogPostId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bt => bt.Topic).WithMany(b => b.BlogTopics).HasForeignKey(bt => bt.TopicId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
