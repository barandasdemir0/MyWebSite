using System.Net;
using System.Text.Json;

namespace WebUILayer.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hata oluştu: {message}", ex.Message);

            // Eğer bu bir validasyon hatasıysa (JSON içeriği varsa) 
            // Middleware bunu yakalayıp Redirect yapmamalı!
            if (IsValidationException(ex.Message))
            {
                // Hatayı fırlat ki ValidationExceptionFilter yakalayıp ModelState'e basabilsin
                throw;
            }

            if (httpContext.Request.Path.StartsWithSegments("/api"))
            {
                // API istekleri için JSON dön
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Sunucu hatası" });
            }
            else
            {
                // Gerçekten beklenmedik bir hataysa yönlendir
                httpContext.Response.Redirect("/Home/Error");
            }
        }
    }

    private bool IsValidationException(string message)
    {
        // API'den gelen mesajın bir ValidationProblemDetails (JSON) olup olmadığını kontrol et
        return message.Trim().StartsWith("{") && message.Contains("errors");
    }

}
