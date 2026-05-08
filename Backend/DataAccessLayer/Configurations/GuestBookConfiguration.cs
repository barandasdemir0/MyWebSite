using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class GuestBookConfiguration : IEntityTypeConfiguration<GuestBook>
{
    public void Configure(EntityTypeBuilder<GuestBook> builder)
    {
        builder.ToTable("GuestBooks");
        builder.HasKey(gb => gb.Id);

        builder.Property(x => x.AuthProvider).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.AuthProviderId).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.AuthorName).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.AuthorAvatarUrl).HasMaxLength(2000);
        builder.Property(x => x.AuthorProfileUrl).HasMaxLength(2000);
        builder.Property(x => x.Message).IsRequired();

    }
}
