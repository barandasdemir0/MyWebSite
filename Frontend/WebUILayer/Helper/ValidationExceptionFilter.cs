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
        string message = context.Exception.Message;
        try
        {
            var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(message,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (problemDetails != null && problemDetails.Errors != null)
            {
                foreach (var error in problemDetails.Errors)
                {
                    foreach (var msg in error.Value)
                    {
                        context.ModelState.AddModelError(error.Key, msg);
                    }
                }
                var result = new ViewResult
                {
                    ViewName = context.RouteData.Values["action"]?.ToString(),
                    ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                };

                context.Result = result;
                context.ExceptionHandled = true;
            }

        }
        catch { }
    }
}
