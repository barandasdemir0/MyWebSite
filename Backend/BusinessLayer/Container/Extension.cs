using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using BusinessLayer.ValidationRules.AboutValidator;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Context;
using DtoLayer.Mapping;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            //mapster için
            // DtoLayer assembly'sindeki TÜM IRegister'ları tarar (AboutMapping referans noktası)
            var config = TypeAdapterConfig.GlobalSettings;
            TypeAdapterConfig.GlobalSettings.Scan(typeof(IMapperMarker).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddValidatorsFromAssemblyContaining<IValidationMarker>();
            //services.AddFluentValidationAutoValidation();
            services.AddFluentValidationAutoValidation();





        }

        //public static void AddScalarConfiguration(this IServiceCollection services)
        //{

        //}


























        public static void ContainerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAboutDal, EfAboutDal>();
            services.AddScoped<IAboutService, AboutManager>();
            services.AddScoped<IBlogPostDal, EfBlogPostDal>();
            services.AddScoped<IBlogPostService, BlogPostManager>();
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
