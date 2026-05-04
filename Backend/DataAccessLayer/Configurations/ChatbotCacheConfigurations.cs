using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ChatbotCacheConfigurations : IEntityTypeConfiguration<ChatbotCache>
{
    public void Configure(EntityTypeBuilder<ChatbotCache> builder)
    {
        builder.ToTable("ChatbotCaches");
        builder.HasKey(x => x.Id);

        // PERFORMANS İÇİN INDEX EKLENTİSİ
        builder.HasIndex(x => x.UserQuestion).IsUnique(false);
    }
}
