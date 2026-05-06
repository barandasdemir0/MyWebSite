using BusinessLayer.Abstract;
using BusinessLayer.Services;
using BusinessLayer.ValidationRules;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Context;
using DtoLayer.Mapping;
using EntityLayer.Constants;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Shared;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;
using System.Threading.RateLimiting;


namespace BusinessLayer.Container;

public static class Extension
{

    public static void AddDatabaseLayers(this IServiceCollection services, IConfiguration configuration)
    {
        //bu kod veritabanına bağlama kodu 
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });
    }

    public static void AddThirdPartyServices(this IServiceCollection services,IConfiguration configuration)
    {
        //mapster için
        // DtoLayer assembly'sindeki TÜM IRegister'ları tarar (AboutMapping referans noktası)
        var config = TypeAdapterConfig.GlobalSettings;
        TypeAdapterConfig.GlobalSettings.Scan(typeof(IMapperMarker).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddValidatorsFromAssemblyContaining<IValidationMarker>();
        services.AddFluentValidationAutoValidation();

        services.AddRateLimiter(options =>
        {
            // 1. Auth Koruması (IP Başına)
            options.AddPolicy(RateLimitConsts.Auth, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(5)
                    }));
            // 2. Ziyaretçi Defteri Koruması (IP Başına)
            options.AddPolicy(RateLimitConsts.GuestBook, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1,
                        Window = TimeSpan.FromMinutes(1)
                    }));
            // 3. İletişim Formu Koruması (IP Başına)
            options.AddPolicy(RateLimitConsts.Message, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromMinutes(5)
                    }));

            // 4. Email Şifre Sıfırlama Koruması (IP Başına)
            options.AddPolicy(RateLimitConsts.Email, context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(1)
                    }));


            
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100, // 1 dakikada max 100 istek
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));


            // Github metodu için daha sert bir kural (Örn: dakikada 5 istek):
            options.AddPolicy("GithubLimit", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });



        services.AddHttpClient("GithubApi", client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent", "MyWebSite-App");
        });


        services.Configure<AdminSettings>(configuration.GetSection("AdminSettings"));

    }

    public static void ContainerDependencies(this IServiceCollection services)
    {

        services.Scan(scan => scan.FromAssemblyOf<IDalMarker>().AddClasses(c => c.Where(t => t.Name.StartsWith("Ef") && t.Name.EndsWith("Dal"))).AsImplementedInterfaces().WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblyOf<IBusinessMarker>().AddClasses(c => c.Where(t => t.Name.EndsWith("Manager"))).AsImplementedInterfaces().WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<EncryptionService>();

    }


    //identity ve jwt için tanımlamalar 
    public static void AddIdentityAndJwt(this IServiceCollection services, IConfiguration configuration)
    {
        //ıdentity
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 16; // En az 16 karakter
            options.Password.RequireNonAlphanumeric = true; // En az bir özel karakter
            options.SignIn.RequireConfirmedEmail = true; // E-posta doğrulaması zorunlu


            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // 15 dakika kilitlenme süresi
            options.Lockout.MaxFailedAccessAttempts = 5; // 5 başarısız giriş denemesinden sonra kilitle
            options.Lockout.AllowedForNewUsers = true; // Yeni kullanıcılar için kilitleme aktif
        })
            .AddEntityFrameworkStores<AppDbContext>() // Identity'nin kullanıcı ve rol yönetimi için AppDbContext'i kullanmasını sağlar
            .AddDefaultTokenProviders(); // Parola sıfırlama ve e-posta doğrulama gibi işlemler için varsayılan token sağlayıcılarını ekler


        //jwt
        var jwtKey = configuration["Jwt:SecretKey"]!; // JWT gizli anahtarını yapılandırmadan alır (appsettings.json'da tanımlanmalıdır)
        services.AddAuthentication(options => // Authentication seçeneklerini yapılandırır
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // JWT Bearer şemasını varsayılan kimlik doğrulama şeması olarak ayarlar
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // JWT Bearer şemasını varsayılan zorlama şeması olarak ayarlar
        })
            .AddJwtBearer(options => // JWT Bearer kimlik doğrulama seçeneklerini yapılandırır
            {
                options.TokenValidationParameters = new TokenValidationParameters // JWT token doğrulama parametrelerini ayarlar
                {
                    ValidateIssuer = true, // Token'ın geçerli bir yayıncı tarafından oluşturulup oluşturulmadığını doğrular
                    ValidateAudience = true, // Token'ın geçerli bir hedef kitleye sahip olup olmadığını doğrular
                    ValidateLifetime = true, // Token'ın süresinin dolup dolmadığını doğrular
                    ValidateIssuerSigningKey = true, // Token'ın imzalama anahtarının geçerli olup olmadığını doğrular
                    ValidIssuer = configuration["Jwt:Issuer"], // JWT yayıncısını yapılandırmadan alır (appsettings.json'da tanımlanmalıdır)
                    ValidAudience = configuration["Jwt:Audience"], // JWT hedef kitlesini yapılandırmadan alır (appsettings.json'da tanımlanmalıdır)
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), // JWT imzalama anahtarını oluşturur (gizli anahtarı UTF-8 byte dizisine dönüştürür)
                    ClockSkew = TimeSpan.Zero // Token'ın geçerlilik süresine ekstra bir tolerans ekler (genellikle 5 dakika varsayılan olarak eklenir, burada sıfır yaparak bu toleransı kaldırıyoruz)
                };
            });
    }

    public static void AddDataProtectionConfig(this IServiceCollection services)
    {
        var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "keys");

        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName("MyWebSite");
    }

    public static void CorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        // CORS politikası ekler, "Cors:AllowedOrigins" yapılandırma bölümünden izin verilen kökenleri alır
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        // "AllowFrontend" adlı bir CORS politikası tanımlar, belirtilen kökenlere, herhangi bir başlığa ve herhangi bir HTTP yöntemine izin verir
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy => // CORS politikasını yapılandırır
            policy.WithOrigins(origins) // Belirtilen kökenlere izin verir (örneğin, "http://localhost:3000" gibi)
            .AllowAnyHeader() // Herhangi bir HTTP başlığına izin verir
            .AllowAnyMethod()
            .AllowCredentials()); // Herhangi bir HTTP yöntemine izin verir (GET, POST, PUT, DELETE vb.)
        });
    }

}
