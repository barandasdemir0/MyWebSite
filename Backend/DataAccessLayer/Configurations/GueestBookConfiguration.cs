using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class GueestBookConfiguration : IEntityTypeConfiguration<GuestBook>
    {
        public void Configure(EntityTypeBuilder<GuestBook> builder)
        {
            builder.ToTable("GuestBooks");
            builder.HasKey(gb => gb.Id);

            builder.Property(x => x.AuthProvider).IsRequired().HasMaxLength(50);
            builder.Property(x => x.AuthProviderId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AuthorName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AuthorAvatarUrl).HasMaxLength(500);
            builder.Property(x => x.AuthorProfileUrl).HasMaxLength(300);
            builder.Property(x => x.Message).IsRequired();

        }
    }
}
