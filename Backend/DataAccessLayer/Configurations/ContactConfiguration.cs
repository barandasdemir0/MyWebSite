using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(c => c.Id);

            builder.Property(x=>x.Email).IsRequired().HasMaxLength(75);
            builder.Property(x=>x.Phone).IsRequired().HasMaxLength(20);
            builder.Property(x=>x.Location).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.LocationPicture).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.ContactTitle).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.ContactText).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.SuccessMessageText).IsRequired().HasMaxLength(150);

            
        }
    }
}
