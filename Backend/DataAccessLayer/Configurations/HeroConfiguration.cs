using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class HeroConfiguration : IEntityTypeConfiguration<Hero>
{
    public void Configure(EntityTypeBuilder<Hero> builder)
    {
        builder.ToTable("Heroes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ScrollingText).HasMaxLength(1000);
        builder.Property(x => x.HeroAbout).HasMaxLength(2000);
        builder.Property(x => x.ProfessionalTitle).HasMaxLength(150);
    }
}
