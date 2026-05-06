using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class LogConfigurations : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        // Id için otomatik Guid üret (SQL tarafında newid() karşılığı)
        builder.Property(e => e.Id).HasDefaultValueSql("newid()");

        // CreatedAt için otomatik tarih (SQL tarafında getdate() karşılığı)
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");

        // IsDeleted her zaman varsayılan olarak false (0) olsun
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
    }
}
