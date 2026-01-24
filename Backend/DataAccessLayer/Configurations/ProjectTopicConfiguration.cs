using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class ProjectTopicConfiguration : IEntityTypeConfiguration<ProjectTopic>
    {
        public void Configure(EntityTypeBuilder<ProjectTopic> builder)
        {
            builder.HasKey(pt => new { pt.ProjectId, pt.TopicId });

            //project ilişkisi 

            builder.HasOne(pt => pt.Project).WithMany(p=>p.ProjectTopics).HasForeignKey(pt=>pt.ProjectId).OnDelete(DeleteBehavior.Cascade);

            //topic ilişkisi

            builder.HasOne(pt=>pt.Topic).WithMany(t=>t.ProjectTopics).HasForeignKey(pt=>pt.TopicId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
