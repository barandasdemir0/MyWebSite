using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace WebUILayer.Extension;

public static class ControllerExtension
{
    //prefix ön eki demektir 
    public static void AddApiError(this ModelStateDictionary modelState, Exception ex, string prefix = "")
    {
        string keyPrefix;
        if (string.IsNullOrEmpty(prefix))
        {
            keyPrefix = "";
        }
        else
        {
            keyPrefix = prefix + ".";
        }
            try
            {
                var problem = JsonSerializer.Deserialize<ValidationProblemDetails>
                    (
                    ex.Message,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if (problem?.Errors != null)
                {
                    foreach (var error in problem.Errors)
                    {
                        foreach (var msg in error.Value)
                        {
                            modelState.AddModelError(keyPrefix + error.Key, msg);
                        }
                    }
                    return; // Hataları ekledik, alttaki genel mesajı ekleme
                }
            }
            catch { }
        modelState.AddModelError("", ex.Message);
    }

    public static async Task<IActionResult> SafeAction(this Controller controller,Func<Task> action,string successMessage,string ErrorMessage, string redirectAction = "Index")
    {
        try
        {
            await action();
            controller.TempData["Success"] = successMessage;
        }
        catch (Exception)
        {

            controller.TempData["Error"] = ErrorMessage;
        }
        return controller.RedirectToAction(redirectAction);
    }
}
