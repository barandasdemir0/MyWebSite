using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable("Experiences");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ExperienceTitle)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.ExperienceCompanyName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(e => e.ExperienceDescription)
            .IsRequired();
    }
}
