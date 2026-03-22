using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RoleName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Permission).HasMaxLength(128).IsRequired();
        builder.HasIndex(x => new
        {
            x.RoleName,
            x.Permission
        }).IsUnique();
    }
}
