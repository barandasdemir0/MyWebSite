using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class AboutConfiguration : IEntityTypeConfiguration<About>
{
    public void Configure(EntityTypeBuilder<About> builder)
    {
        builder.ToTable("About");
        builder.HasKey(x=> x.Id);
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
        builder.Property(x=>x.Greeting).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Bio).IsRequired().HasMaxLength(2000);
        
    }
}
