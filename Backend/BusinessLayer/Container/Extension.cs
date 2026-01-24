using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Container
{
    public static class Extension
    {

        public static void AddDatabaseLayers(this IServiceCollection services,IConfiguration configuration)
        {
            //bu kod veritabanına bağlama kodu 
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });



        }


        public static void AddThirdPartyServices(this IServiceCollection services)
        {

        }


























        public static void ContainerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAboutDal, EfAboutDal>();
            services.AddScoped<IBlogPostDal, EfBlogPostDal>();
            services.AddScoped<ICertificateDal, EfCertificateDal>();
            services.AddScoped<IContactDal, EfContactDal>();
            services.AddScoped<IEducationDal, EfEducationDal>();
            services.AddScoped<IExperienceDal, EfExperienceDal>();
            services.AddScoped<IGithubRepoDal, EfGithubRepoDal>();
            services.AddScoped<IGuestBookDal, EfGuestBookDal>();
            services.AddScoped<IHeroDal, EfHeroDal>();
            services.AddScoped<IMessageDal, EfMessageDal>();
            services.AddScoped<IProjectDal, EfProjectDal>();
            services.AddScoped<ISkillDal, EfSkillDal>();
            services.AddScoped<ISocialMediaDal, EfSocialMediaDal>();
            services.AddScoped<ITopicDal, EfTopicDal>();
        }




    }
}
