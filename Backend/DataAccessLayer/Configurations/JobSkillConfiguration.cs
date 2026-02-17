using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations;

public sealed class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("JobSkill");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.JobSkillName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.JobSkillPercentage).IsRequired();

        builder.HasOne(x => x.JobSkillCategory).WithMany(y => y.JobSkills).HasForeignKey(x => x.JobSkillCategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}
