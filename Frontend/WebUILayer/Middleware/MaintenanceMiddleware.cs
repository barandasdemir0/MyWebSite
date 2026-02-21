using System.Text.Json;

namespace WebUILayer.Middleware;

public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MaintenanceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";
        if (path.StartsWith("/admin") || path.StartsWith("auth"))
        {
            await _next(context);
            return;
        }


        if (path.StartsWith("/Mytheme") || path.StartsWith("/lib") || path.Contains("."))
        {
            await _next(context);
            return;
        }

        if (path == "/maintenance")
        {
            await _next(context);
            return;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}SiteSettings/single");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                var isMaintenanceMode = doc.RootElement.GetProperty("isMaintenanceMode").GetBoolean();
                if (isMaintenanceMode)
                {
                    context.Response.StatusCode = 503;
                    context.Response.Headers.Append("Retry-After", "3600");
                    context.Response.Redirect("/Mytheme/admin/maintenance.html");
                    return;
                }
            }
        }
        catch (Exception)
        {

            throw;
        }

        await _next(context);



    }








}
