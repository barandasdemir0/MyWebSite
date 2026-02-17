using BusinessLayer.Abstract;
using DtoLayer.ChatbotSettingsDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public class ChatbotSettingsController : CrudController<ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto>
{

    private readonly IChatbotSettingsService _chatbotSettingsService;

    public ChatbotSettingsController(IChatbotSettingsService chatbotSettingsService) : base(chatbotSettingsService)
    {
        _chatbotSettingsService = chatbotSettingsService;
    }
   
}
