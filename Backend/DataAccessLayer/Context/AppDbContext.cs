using CV.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
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
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<JobSkillCategory> JobSkillCategories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTopic> ProjectTopics { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }
        public DbSet<ChatbotSettings> ChatbotSettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        //global query filter otomatik soft delete filtresi
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //bu configurasyon dosyalarımızı bulup otomatik uygular hani biz fluentapi yaptık ya

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                //Bu entity BaseEntity'den türüyor mu?" kontrolü. eğer türemiyorsaki join tabloları gibi atla çünkü ara tabloda soft delete gerek yok
                {
                    var parameter = Expression.Parameter(entityType.ClrType, ("entity"));
                    //Sonuç: entity => kısmı oluştu.  Lambda'nın parametresini oluştur.
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));// güvenli
                    //entity.IsDeleted property erişimini oluştur.
                    // nameof ile yazarsan, derleme zamanında kontrol edilir:  
                    var falseConstant  = Expression.Constant(false);//false değerini oluştur. Karşılaştırmada kullanılacak.
                    var condition = Expression.Equal(property, falseConstant);
                    //entity.IsDeleted == false karşılaştırmasını oluştur.
                    var lambda = Expression.Lambda(condition, parameter);
                    //Her şeyi lambda expression olarak paketle
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                    //Oluşturulan lambda'yı bu entity'nin global filtresi olarak ata.


                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:  // YENİ KAYIT
                        entry.Entity.CreatedAt = DateTime.UtcNow;// Oluşturulma tarihi ata
                        break;
                    case EntityState.Modified:// GÜNCELLEME
                        entry.Entity.UpdatedAt = DateTime.UtcNow;// Güncellenme tarihi ata
                        break;

                    case EntityState.Deleted:   // SİLME (Remove çağrıldığında)
                        entry.State = EntityState.Modified; // ❗ Silme → Güncelleme'ye çevir
                        entry.Entity.IsDeleted = true;// IsDeleted = true yap
                        entry.Entity.DeletedAt = DateTime.UtcNow; // Silinme tarihi ata
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

   
    
    
}
