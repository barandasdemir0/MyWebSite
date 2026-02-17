using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class SiteSettingsConfiguration : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        builder.ToTable("SiteSettings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkStatus).HasMaxLength(100);
        builder.Property(x => x.CvFileUrlTr).HasMaxLength(500);
        builder.Property(x => x.CvFileUrlEn).HasMaxLength(500);
        // Yeni SEO Alanları
        builder.Property(x => x.SiteTitle).HasMaxLength(200);
        builder.Property(x => x.MetaDescription).HasMaxLength(500);
        builder.Property(x => x.GoogleAnalyticsId).HasMaxLength(50);
        builder.Property(x => x.SiteKeywords).HasMaxLength(300);
    }
}
