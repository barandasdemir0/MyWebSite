using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicFooterViewComponent:ViewComponent
{
    private readonly IContactApiService _publicContactApiService;
    private readonly IPublicSocialMediaApiService _publicSocialMediaApiService;

    public LayoutPublicFooterViewComponent(IContactApiService publicContactApiService, IPublicSocialMediaApiService publicSocialMediaApiService)
    {
        _publicContactApiService = publicContactApiService;
        _publicSocialMediaApiService = publicSocialMediaApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var contact = await _publicContactApiService.GetAllAsync();
        
        
        var model = new FooterViewModel
        {
            Contacts = contact.FirstOrDefault(),
            SocialMedias = await _publicSocialMediaApiService.GetAllAsync()
        };
        return View(model);
    }
}
