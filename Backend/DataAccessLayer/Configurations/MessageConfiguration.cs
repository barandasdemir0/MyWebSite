using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using static EntityLayer.Concrete.Message;

namespace DataAccessLayer.Configurations
{
    public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.Id);
            // Gönderen
            builder.Property(x => x.SenderName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.SenderEmail).IsRequired().HasMaxLength(100);
            // Alıcı
            builder.Property(x => x.ReceiverEmail).IsRequired().HasMaxLength(100);
            // Mesaj
            builder.Property(x => x.Subject).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Body).IsRequired();
            // Enum string olarak sakla
            builder.Property(x => x.Folder)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(MessageFolder.Inbox);
            // Index'ler
            builder.HasIndex(x => x.SenderEmail);
            builder.HasIndex(x => x.Folder);
        }
    }
}
