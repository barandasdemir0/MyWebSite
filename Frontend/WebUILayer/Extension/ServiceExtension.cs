using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Helper;
using WebUILayer.Services.Abstract;
using WebUILayer.Services.Concrete;

namespace WebUILayer.Extension;

public static class ServiceExtension
{
    public static void AddApiService(this IServiceCollection services, IConfiguration configuration)
    {
        var baseurl = configuration["ApiSettings:BaseUrl"];

        #region admin için bağlamalar  --> bu yapı daha sonra scrutor ile otomatikleştirilecek

        services.ConfigureHttpClientDefaults(builder =>
        {
            builder.ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseurl!);
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        });

        services.AddTransient<JwtTokenHandler>();


        services.AddHttpClient<IAboutApiService, AboutApiService>().AddHttpMessageHandler<JwtTokenHandler>();


        services.AddHttpClient<IBlogPostApiService, BlogPostApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ITopicApiService, TopicApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IHeroApiService, HeroApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ICertificateApiService, CertificateApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IEducationApiService, EducationApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IExperienceApiService, ExperienceApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IProjectApiService, ProjectApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ISkillApiService, SkillApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ISocialMediaApiService, SocialMediaApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IJobSkillApiService, JobSkillApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IJobSkillCategoryService, JobSkillCategoryApiService>().AddHttpMessageHandler<JwtTokenHandler>();
        services.AddHttpClient<ISiteSettingsApiService, SiteSettingsApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IContactApiService, ContactApiService>().AddHttpMessageHandler<JwtTokenHandler>();



        services.AddHttpClient<IGithubApiService, GithubRepoApiService>().AddHttpMessageHandler<JwtTokenHandler>();



        services.AddHttpClient<IUserProfileApiService, UserProfileApiService>().AddHttpMessageHandler<JwtTokenHandler>();


        services.AddHttpClient<IUserAdminApiService, UserAdminApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IRolePermissionApiService, RolePermissionApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IMessageApiService, MessageApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IGuestBookApiService, GuestBookApiService>().AddHttpMessageHandler<JwtTokenHandler>();




        #endregion

        #region public için bağlamalar --> bu yapı scrutor ile otomatikleştirilecek
        services.AddScoped<ICookieAuthService, CookieAuthService>();

        services.AddHttpClient<IAuthApiService, AuthApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<ITwoFactorApiService, TwoFactorApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IAccountApiService, AccountApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<IPublicMessageApiService, PublicMessageApiService>();

        services.AddHttpClient<IOAuthApiService, OAuthApiService>();


        services.AddHttpClient<IPublicAboutApiService, PublicAboutApiService>();

        services.AddHttpClient<IPublicContactApiService, PublicContactApiService>();

        services.AddHttpClient<IPublicJobSkillApiService, PublicJobSkillApiService>();

        services.AddHttpClient<IPublicJobSkillCategoryService, PublicJobSkillCategoryService>();

        services.AddHttpClient<IPublicSiteSettingsApiService, PublicSiteSettingsApiService>();

        services.AddHttpClient<IPublicHeroApiService, PublicHeroApiService>();

        services.AddHttpClient<IPublicSocialMediaApiService, PublicSocialMediaApiService>();

        services.AddHttpClient<IPublicSkillApiService, PublicSkillApiService>();

        services.AddHttpClient<IPublicProjectApiService, PublicProjectApiService>();

        services.AddHttpClient<IPublicGuestBookApiService, PublicGuestBookApiService>();

        services.AddHttpClient<IPublicGithubApiService, PublicGithubApiService>();

        services.AddHttpClient<IPublicCertificateApiService, PublicCertificateApiService>();

        services.AddHttpClient<IPublicEducationApiService, PublicEducationApiService>();

        services.AddHttpClient<IPublicExperienceApiService, PublicExperienceApiService>();

        services.AddHttpClient<IPublicBlogPostApiService, PublicBlogPostApiService>();

        services.AddHttpClient<IPublicTopicApiService, PublicTopicApiService>();
        services.AddScoped<IGuestSessionService, GuestSessionService>();



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
            options.Cookie.SameSite = SameSiteMode.Strict; // EKLENDİ
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // EKLENDİ
            options.IdleTimeout = TimeSpan.FromMinutes(5);
        });
    }


}
