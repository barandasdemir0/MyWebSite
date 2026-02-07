using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.ViewComponents.AdminComponents;

public class _ResumeEducationPartialComponent:ViewComponent
{
    private readonly IEducationApiService _educationApiService;

    public _ResumeEducationPartialComponent(IEducationApiService educationApiService)
    {
        _educationApiService = educationApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _educationApiService.GetAllAdminAsync();
        return View(result);
    }
}
