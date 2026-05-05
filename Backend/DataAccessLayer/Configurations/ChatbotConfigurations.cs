using CV.EntityLayer.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public sealed class ChatbotConfigurations : IEntityTypeConfiguration<ChatbotSettings>
{

    private readonly IDataProtector _protector;

    public ChatbotConfigurations(IDataProtector protector)
    {
        _protector = protector;
    }

    public void Configure(EntityTypeBuilder<ChatbotSettings> builder)
    {
        builder.ToTable("ChatbotSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AssistantName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.WelcomeMessage).HasMaxLength(500);
        builder.Property(x => x.SystemPrompt).HasMaxLength(2000); // Prompt uzun olabilir
        builder.Property(e => e.ApiKey).HasMaxLength(500).HasConversion(
           v => v == null ? null : _protector.Protect(v),       // Yazarken şifrele
            v => v == null ? null : _protector.Unprotect(v)     // Okurken çöz (Uygulama düz metin görür, DB şifreli)
        );
        builder.Property(x => x.ModelName).HasMaxLength(100);
    }
}
