using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace WebUILayer.Extension;

public static class ControllerExtension
{
    public static void AddApiError(this ModelStateDictionary modelState ,Exception ex)
    {
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
						modelState.AddModelError(error.Key, msg);
					}
				}
			}
		}
		catch {}
		modelState.AddModelError("", ex.Message);
    }
}
