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
    }
}
