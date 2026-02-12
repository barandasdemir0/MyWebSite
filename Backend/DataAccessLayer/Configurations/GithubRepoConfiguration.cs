using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configurations
{
    public sealed class GithubRepoConfiguration : IEntityTypeConfiguration<GithubRepo>
    {
        public void Configure(EntityTypeBuilder<GithubRepo> builder)
        {
            builder.ToTable("GithubRepos");
            builder.HasKey(gr => gr.Id);

            builder.Property(gr => gr.RepoName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(gr => gr.Description).HasMaxLength(500);
            builder.Property(gr => gr.Language).HasMaxLength(500);
            builder.Property(gr => gr.RepoUrl).HasMaxLength(500);
        }
    }
}
