using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations;

public sealed class ChatbotConfigurations : IEntityTypeConfiguration<ChatbotSettings>
{
    public void Configure(EntityTypeBuilder<ChatbotSettings> builder)
    {
        builder.ToTable("ChatbotSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AssistantName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.WelcomeMessage).HasMaxLength(500);
        builder.Property(x => x.SystemPrompt).HasMaxLength(2000); // Prompt uzun olabilir
        builder.Property(x => x.ApiKey).HasMaxLength(500); // Şifreli saklanacaksa uzun olabilir
        builder.Property(x => x.ModelName).HasMaxLength(100);
    }
}
