using Microsoft.AspNetCore.Mvc;
using WebUILayer.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicHeaderViewComponent : ViewComponent
{
    private readonly IPublicSiteSettingsApiService _publicSiteSettingsApiService;

    public LayoutPublicHeaderViewComponent(IPublicSiteSettingsApiService publicSiteSettingsApiService)
    {
        _publicSiteSettingsApiService = publicSiteSettingsApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await _publicSiteSettingsApiService.GetPublicSiteSettingsAsync();
        return View(values);
    }
}
