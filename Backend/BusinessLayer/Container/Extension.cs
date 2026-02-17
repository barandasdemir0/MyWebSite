using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
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
        //services.AddFluentValidationAutoValidation();
        services.AddFluentValidationAutoValidation();





    }



    //public static void AddScalarConfiguration(this IServiceCollection services)
    //{

    //}


























    public static void ContainerDependencies(this IServiceCollection services)
    {

        services.Scan(scan => scan.FromAssemblyOf<IDalMarker>().AddClasses(c => c.Where(t => t.Name.StartsWith("Ef") && t.Name.EndsWith("Dal"))).AsImplementedInterfaces().WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblyOf<IBusinessMarker>().AddClasses(c => c.Where(t => t.Name.EndsWith("Manager"))).AsImplementedInterfaces().WithScopedLifetime());


     

    }




}
