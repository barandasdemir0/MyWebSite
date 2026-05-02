using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUILayer.Helper;

public class ValidationExceptionFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _modelMetadataProvider;

    public ValidationExceptionFilter(IModelMetadataProvider modelMetadataProvider)
    {
        _modelMetadataProvider = modelMetadataProvider;
    }

    public void OnException(ExceptionContext context)
    {
        // Exception'ın null olma ihtimaline karşı uyarıyı susturmak için ! (null forgiving) veya kontrol ekliyoruz.
        if (context.Exception == null)
        {
            return;
        }

        string message = context.Exception.Message;

        try
        {
            var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (problemDetails != null)
            {
                if (problemDetails.Errors != null)
                {
                    // Hataları ModelState içine dolduruyoruz
                    foreach (var error in problemDetails.Errors)
                    {
                        // Derleyicinin "Ya Value null gelirse?" uyarısını yok etmek için null kontrolü yapıyoruz
                        if (error.Value != null)
                        {
                            foreach (var msg in error.Value)
                            {
                                context.ModelState.AddModelError(error.Key, msg);
                            }
                        }
                    }

                    // ViewData nesnesini klasik yöntemle açık açık oluşturuyoruz
                    ViewDataDictionary yeniViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);

                    // Hangi sayfada (action) olduğumuzu buluyoruz (Örn: Create, Update)
                    string sayfaAdi = "";
                    var actionDegeri = context.RouteData.Values["action"];

                    if (actionDegeri != null)
                    {
                        // .ToString() sonucunun null dönme uyarısını ?? "" ile çözüyoruz
                        sayfaAdi = actionDegeri.ToString() ?? "";
                    }

                    // ViewResult nesnesini oluşturup View'a gönderiyoruz
                    ViewResult result = new ViewResult();
                    result.ViewName = sayfaAdi;
                    result.ViewData = yeniViewData;

                    context.Result = result;
                    context.ExceptionHandled = true; // Hatayı yakaladık, sistem çökmesin diyoruz
                }
            }
        }
        catch
        {
            // JSON dönüştürmede bir hata olursa sistem çökmeyecek, filtre iptal olacak
        }
    }
}
