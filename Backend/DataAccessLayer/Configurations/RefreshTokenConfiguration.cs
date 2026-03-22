using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Token).HasMaxLength(128).IsRequired();
        builder.Property(x => x.DeviceInfo).HasMaxLength(512);
        builder.HasIndex(x => new { x.Token, x.UserId });
        builder.HasIndex(x => x.ExpiresAt);
        builder.HasOne(x => x.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
