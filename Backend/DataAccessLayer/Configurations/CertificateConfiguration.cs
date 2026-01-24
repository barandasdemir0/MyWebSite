using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");
            builder.HasKey(c => c.Id);

            builder.Property(x=>x.CertificateName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IssuingCompany).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.CertificateDescription).IsRequired();
        }
    }
}
