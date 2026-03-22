using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using DtoLayer.Mapping;
using EntityLayer.Entities;
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

    public static void AddIdentityAndJwt(this IServiceCollection services, IConfiguration configuration)
    {
        //ıdentity
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 16;
            options.Password.RequireNonAlphanumeric = true;
            options.SignIn.RequireConfirmedEmail = true;


            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        //jwt
        var jwtKey = configuration["Jwt:SecretKey"]!;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void AddEmailRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("email", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1);
                opt.PermitLimit = 3;
            });
        });
    }

    public static void CorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {

        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod());
        });
    }

}
