using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Helper;
using WebUILayer.Services.Abstract;
using WebUILayer.Services.Concrete;
using WebOptimizer;



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
        services.AddHttpClient<IChatbotSettingsApiService, ChatbotSettingsApiService>().AddHttpMessageHandler<JwtTokenHandler>();

        services.AddHttpClient<INotificationApiService, NotificationApiService>().AddHttpMessageHandler<JwtTokenHandler>();
        services.AddHttpClient<ILogApiService, LogApiService>().AddHttpMessageHandler<JwtTokenHandler>();






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

            // KRİTİK: Yazdığın filtreyi buraya ekle!
            options.Filters.Add<ValidationExceptionFilter>();
        });
    }


    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        // Sistemdeki "Unable to find required services" hatasını bu satır çözer.
        services.AddAuthorization();
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
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
        });
    }


    public static void AddSessionTempData(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax; // EKLENDİ
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // EKLENDİ
            options.IdleTimeout = TimeSpan.FromHours(5);
        });
    }

    public static void AddPerformanceOptimization(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = new[]
            {
            "text/html", "text/css", "application/javascript",
            "application/json", "image/svg+xml", "text/plain"
            };
        });
    }

    public static void AddWebOptimization(this IServiceCollection services)
    {
        services.AddWebOptimizer(pipeline =>
        {
            // TÜM CSS dosyalarını TEK bir bundle'a topla
            pipeline.AddCssBundle("/css/bundle.css",
                // 1. Base (önce yüklenmeli)
                "/Mytheme/portfolio/css/base/variables.css",
                "/Mytheme/portfolio/css/base/reset.css",

                // 2. Components
                "/Mytheme/portfolio/css/components/navbar.css",
                "/Mytheme/portfolio/css/components/buttons.css",
                "/Mytheme/portfolio/css/components/cards.css",
                "/Mytheme/portfolio/css/components/forms.css",
                "/Mytheme/portfolio/css/components/footer.css",
                "/Mytheme/portfolio/css/components/modal.css",
                "/Mytheme/portfolio/css/components/language.css",
                "/Mytheme/portfolio/css/components/chatbot.css",
                "/Mytheme/portfolio/css/components/availability.css",
                "/Mytheme/portfolio/css/components/testimonials.css",
                "/Mytheme/portfolio/css/components/search.css",
                "/Mytheme/portfolio/css/components/related-content.css",

                // 3. Sections
                "/Mytheme/portfolio/css/sections/hero.css",
                "/Mytheme/portfolio/css/sections/sections.css",
                "/Mytheme/portfolio/css/sections/marquee.css",
                "/Mytheme/portfolio/css/sections/github.css",
                "/Mytheme/portfolio/css/sections/guestbook.css",
                "/Mytheme/portfolio/css/sections/blog.css",
                "/Mytheme/portfolio/css/sections/blog-details.css",
                "/Mytheme/portfolio/css/sections/project-details.css",

                // 4. Pages
                "/Mytheme/portfolio/css/pages/contact.css",
                "/Mytheme/portfolio/css/pages/project-detail.css",
                "/Mytheme/portfolio/css/pages/resume.css",

                // 5. Theme & Responsive (en sonda — override eder)
                "/Mytheme/portfolio/css/themes.css",
                "/Mytheme/portfolio/css/responsive.css"
            );

            //// JS dosyalarını ayrı ayrı küçült (bundle yapmaya gerek yok)
            pipeline.MinifyJsFiles("/Mytheme/portfolio/js/animations.js",
                                   "/Mytheme/portfolio/js/main.js",
                                   "/Mytheme/portfolio/js/github.js",
                                   "/Mytheme/portfolio/js/language.js",
                                   "/Mytheme/portfolio/js/chatbot.js",
                                   "/Mytheme/portfolio/js/theme.js",
                                   "/js/site.js");
        });
    }



}
