using Microsoft.AspNetCore.Mvc;
using WebUILayer.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicScriptsViewComponent : ViewComponent
{
    private readonly IPublicSiteSettingsApiService _publicSiteSettingsApiService;

    public LayoutPublicScriptsViewComponent(IPublicSiteSettingsApiService publicSiteSettingsApiService)
    {
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await _publicSiteSettingsApiService.GetAllAsync();
        return View(values);
    }
}
