using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Token).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => new 
        { 
            x.Token, x.Email 
        });
    }
}
