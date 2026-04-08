using SharedKernel.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApiLayer.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;// birsonraki middlewareyi temsil eder
    private readonly ILogger<ApiExceptionMiddleware> _logger; //loglamak için

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {//Her HTTP isteğinin içinden geçtiği ara katmana middleware deriz
        try
        {
            await _next(context); // eğer aşağıda bir exception olursa buraya geri düş
        }
        catch (NotFoundException ex) //exceptionu yakala
        {
            _logger.LogWarning(ex, "Kayıt bulunamadı : {Message}", ex.Message);//hata bilgisi ve mesajı kaydet
            context.Response.StatusCode = (int)HttpStatusCode.NotFound; //istenen kaynak yok
            context.Response.ContentType = "application/json"; //gönderilecek tip json
            await context.Response.WriteAsync(JsonSerializer.Serialize(new // C# objesi error ) ex.message ama client error = user bulunamadı gibi bir değer göknderiyor 
            {
                error = ex.Message
            }));
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "İş kuralı hatası : {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = ex.Message
            }));
        }
        catch(ValidationException ex)
        {
            _logger.LogWarning(ex, "Validasyon Hatası:{Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = ex.Message
            }));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Beklenmeyen hata : {Message}", ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new //object json stringe çevirdik
            {
                error = "Sunucu hatası oluştu"
            }));
        }
    }


}
