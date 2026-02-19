using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;

namespace WebUILayer.Extension;

public static class ServiceExtension
{
    public static void AddApiService(this IServiceCollection services,IConfiguration configuration)
    {
        var baseurl = configuration["ApiSettings:Baseurl"];

        services.AddHttpClient<IAboutApiService, AboutApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });


        services.AddHttpClient<IBlogPostApiService, BlogPostApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<ITopicApiService, TopicApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IHeroApiService, HeroApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<ICertificateApiService, CertificateApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IEducationApiService, EducationApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IExperienceApiService, ExperienceApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IProjectApiService, ProjectApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<ISkillApiService, SkillApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<ISocialMediaApiService, SocialMediaApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IJobSkillApiService, JobSkillApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IJobSkillCategoryService, JobSkillCategoryApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });
        services.AddHttpClient<ISiteSettingsApiService, SiteSettingsApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });

        services.AddHttpClient<IContactApiService, ContactApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });



        services.AddHttpClient<IGithubApiService, GithubRepoApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        });










    }


    public static void AddAutoValidate(this IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
      
    }


 

}
