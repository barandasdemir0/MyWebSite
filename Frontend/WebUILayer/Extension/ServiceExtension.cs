using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Helper;

namespace WebUILayer.Extension;

public static class ServiceExtension
{
    public static void AddApiService(this IServiceCollection services,IConfiguration configuration)
    {
        var baseurl = configuration["ApiSettings:Baseurl"];

        #region admin için bağlamalar  --> bu yapı daha sonra scrutor ile otomatikleştirilecek

        services.AddTransient<JwtTokenHandler>();


        services.AddHttpClient<IAboutApiService, AboutApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();


        services.AddHttpClient<IBlogPostApiService, BlogPostApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ITopicApiService, TopicApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IHeroApiService, HeroApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ICertificateApiService, CertificateApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IEducationApiService, EducationApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IExperienceApiService, ExperienceApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IProjectApiService, ProjectApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ISkillApiService, SkillApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ISocialMediaApiService, SocialMediaApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IJobSkillApiService, JobSkillApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IJobSkillCategoryService, JobSkillCategoryApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();
        services.AddHttpClient<ISiteSettingsApiService, SiteSettingsApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IContactApiService, ContactApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();



        services.AddHttpClient<IGithubApiService, GithubRepoApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IUserProfileApiService, UserProfileApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ITwoFactorApiService, TwoFactorApiService>(client =>
        {
            client.BaseAddress = new Uri(baseurl!);
        }).AddHttpMessageHandler<JwtTokenHandler>();





        #endregion

        #region public için bağlamalar --> bu yapı scrutor ile otomatikleştirilecek


        #endregion








    }


    public static void AddAutoValidate(this IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });
      
    }




    public static void AddCookieAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.AccessDeniedPath = "/auth/login";
            options.Cookie.Name = "AdminAuth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
        });
    }


    public static void AddSessionTempData(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(5);
        });
    }
 

}
