using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace WebUILayer.Middleware;

public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;

    public MaintenanceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache cache)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _cache = cache;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        if (path.StartsWith("/admin") || path.StartsWith("/auth"))
        {
            await _next(context);
            return;
        }


        if (path.StartsWith("/mytheme") || path.StartsWith("/lib") || path.Contains("."))
        {
            await _next(context);
            return;
        }

        if (path == "/maintenance")
        {
            await _next(context);
            return;
        }
        bool isMaintenanceMode = false;
        try
        {
             isMaintenanceMode = await _cache.GetOrCreateAsync("IsMaintenanceMode", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);


                var client = _httpClientFactory.CreateClient();
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var response = await client.GetAsync($"{baseUrl}SiteSettings/single");
                if (!response.IsSuccessStatusCode)
                    return false;
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);


                if (doc.RootElement.TryGetProperty("isMaintenanceMode", out var prop))
                {
                    return prop.GetBoolean();
                }


                if (doc.RootElement.TryGetProperty("IsMaintenanceMode",out prop))
                {
                    return prop.GetBoolean();
                }
                return false;
            });

           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Maintenance Check Hatası : {ex.Message}");
            isMaintenanceMode = false; 
        } // Hata olursa site açık varsay


        if (isMaintenanceMode)
        {
            context.Response.Redirect("/maintenance");
            return; // Pipeline'ı kes
        }
        await _next(context);



    }








}
