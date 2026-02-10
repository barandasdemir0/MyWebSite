using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations;

public class JobSkillCategoryConfiguration : IEntityTypeConfiguration<JobSkillCategory>
{
    public void Configure(EntityTypeBuilder<JobSkillCategory> builder)
    {
        builder.ToTable("JobSkillCategory");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CategoryDescription).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CategoryIcon).IsRequired().HasMaxLength(100);
    }
}
