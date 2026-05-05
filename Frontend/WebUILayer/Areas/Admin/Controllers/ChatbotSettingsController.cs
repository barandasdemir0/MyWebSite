using DtoLayer.ChatbotSettingsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ChatbotSettingsController : Controller
{
    private readonly IChatbotSettingsApiService _chatbotSettingsApiService;

    public ChatbotSettingsController(IChatbotSettingsApiService chatbotSettingsApiService)
    {
        _chatbotSettingsApiService = chatbotSettingsApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _chatbotSettingsApiService.GetChatbotSettingForEditAsync();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Index(UpdateChatbotSettingsDto updateChatbotSettingsDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateChatbotSettingsDto);
        }
        try
        {


            await _chatbotSettingsApiService.SaveChatbotSettingAsync(updateChatbotSettingsDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // API'den fırlayan 400 BadRequest JSON'unu yakalayıp UI'daki kırmızı span'lara dağıtan sihirli metodun!
            ModelState.AddApiError(ex);
            return View(updateChatbotSettingsDto);
        }
    }
}
