using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("Educations");
        builder.HasKey(x=>x.Id);

        builder.Property(x => x.EducationDegree).IsRequired().HasMaxLength(50);
        builder.Property(x => x.EducationSchoolName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.EducationDescription).IsRequired();

    }
}
