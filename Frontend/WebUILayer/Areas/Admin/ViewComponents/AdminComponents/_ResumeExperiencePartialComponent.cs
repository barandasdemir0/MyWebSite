using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.ViewComponents.AdminComponents;

public class _ResumeExperiencePartialComponent:ViewComponent
{
    private readonly IExperienceApiService _experienceApiService;

    public _ResumeExperiencePartialComponent(IExperienceApiService experienceApiService)
    {
        _experienceApiService = experienceApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var query = await _experienceApiService.GetAllAdminAsync();
        return View(query);
    }
}
