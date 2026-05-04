using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.ViewComponents.LayoutComponents;

public class LayoutPublicChatbotViewComponent:ViewComponent
{
    private readonly IChatbotSettingsApiService _chatbotSettingsApiService;

    public LayoutPublicChatbotViewComponent(IChatbotSettingsApiService chatbotSettingsApiService)
    {
        _chatbotSettingsApiService = chatbotSettingsApiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _chatbotSettingsApiService.GetChatbotSettingForEditAsync();
        return View(settings);
    }
}
