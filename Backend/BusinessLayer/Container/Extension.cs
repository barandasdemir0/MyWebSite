using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using DtoLayer.Mapping;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;


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

    public static void AddThirdPartyServices(this IServiceCollection services)
    {
        //mapster için
        // DtoLayer assembly'sindeki TÜM IRegister'ları tarar (AboutMapping referans noktası)
        var config = TypeAdapterConfig.GlobalSettings;
        TypeAdapterConfig.GlobalSettings.Scan(typeof(IMapperMarker).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddValidatorsFromAssemblyContaining<IValidationMarker>();
        services.AddFluentValidationAutoValidation();



        services.AddHttpClient("GithubApi", client =>
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("User-Agent", "MyWebSite-App");
        });

    }

    public static void ContainerDependencies(this IServiceCollection services)
    {

        services.Scan(scan => scan.FromAssemblyOf<IDalMarker>().AddClasses(c => c.Where(t => t.Name.StartsWith("Ef") && t.Name.EndsWith("Dal"))).AsImplementedInterfaces().WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblyOf<IBusinessMarker>().AddClasses(c => c.Where(t => t.Name.EndsWith("Manager"))).AsImplementedInterfaces().WithScopedLifetime());
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

    //RateLimiter için tanımlamalar
    public static void AddEmailRateLimiter(this IServiceCollection services) 
    {
        // Sabit pencere sınırlayıcısı ekler, "email" adlı bir sınırlayıcı tanımlar
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("email", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1); // Her 1 dakikalık pencere için sınırlama uygular
                opt.PermitLimit = 3; // Her pencere için maksimum 3 izin verir (örneğin, her dakika en fazla 3 e-posta gönderimine izin verir)
            });
        });
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
            .AllowAnyMethod()); // Herhangi bir HTTP yöntemine izin verir (GET, POST, PUT, DELETE vb.)
        });
    }

}
