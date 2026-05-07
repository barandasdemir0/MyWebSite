using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicFooterViewComponent:ViewComponent
{
    private readonly IPublicContactApiService _publicContactApiService;
    private readonly IPublicSocialMediaApiService _publicSocialMediaApiService;

    public LayoutPublicFooterViewComponent(IPublicContactApiService publicContactApiService, IPublicSocialMediaApiService publicSocialMediaApiService)
    {
        _publicContactApiService = publicContactApiService;
        _publicSocialMediaApiService = publicSocialMediaApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        
        
        
        var model = new FooterViewModel
        {
            Contacts = await _publicContactApiService.GetPublicContactSettingsAsync(),
            SocialMedias = await _publicSocialMediaApiService.GetAllAsync()
        };
        return View(model);
    }
}
