using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class SocialMediaConfiguration : IEntityTypeConfiguration<SocialMedia>
    {
        public void Configure(EntityTypeBuilder<SocialMedia> builder)
        {
            builder.ToTable("SocialMedias");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SocialMediaName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.SocialMediaUrl).IsRequired().HasMaxLength(250);
            builder.Property(x => x.SocialMediaIcon).IsRequired().HasMaxLength(100);
        }


    }
}
