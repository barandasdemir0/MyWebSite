using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(x => x.Id);
        // Temel Bilgiler
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
        builder.HasIndex(x => x.Slug).IsUnique();  // Benzersiz URL

        builder.Property(x => x.ShortDescription).HasMaxLength(1000);
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.ImageUrl).HasMaxLength(500);
        // Proje Bilgileri
        builder.Property(x => x.ClientName).HasMaxLength(200);
        builder.Property(x => x.Duration).HasMaxLength(50);
        builder.Property(x => x.Role).HasMaxLength(100);
        // Detaylı Açıklamalar
        builder.Property(x => x.Goals).HasMaxLength(3000);
        // Linkler
        builder.Property(x => x.WebsiteUrl).HasMaxLength(300);
        builder.Property(x => x.GithubUrl).HasMaxLength(300);
        // Teknolojiler
        builder.Property(x => x.Technologies).HasMaxLength(500);
    }
}
