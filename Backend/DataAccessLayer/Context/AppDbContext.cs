using CV.EntityLayer.Entities;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Context
{
    public sealed class AppDbContext:DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<About> About { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogTopic> BlogTopics { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<GithubRepo> GithubRepos { get; set; }
        public DbSet<GuestBook> GuestBooks { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTopic> ProjectTopics { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, ("entity"));
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var falseConstant  = Expression.Constant(false);
                    var condition = Expression.Equal(property, falseConstant);
                    var lambda = Expression.Lambda(condition, parameter)
;

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

   
    
    
}
