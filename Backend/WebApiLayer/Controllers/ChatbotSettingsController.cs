using BusinessLayer.Abstract;
using DtoLayer.ChatbotSettingsDtos;
using EntityLayer.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public class ChatbotSettingsController : PublicCrudController<ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto>
{

    private readonly IChatbotSettingsService _chatbotSettingsService;

    public ChatbotSettingsController(IChatbotSettingsService chatbotSettingsService) : base(chatbotSettingsService)
    {
        _chatbotSettingsService = chatbotSettingsService;
    }
    [AllowAnonymous]
    [HttpGet("single")]
    public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
    {
        var values = await _chatbotSettingsService.GetSingleAsync(cancellationToken);
        if (values==null)
        {
            return NotFound();
        }
        return Ok(values);
    }
    [Authorize(Roles = RoleConsts.Admin)] // Sadece admin güncelleyebilir
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UpdateChatbotSettingsDto updateChatbotSettingsDto,CancellationToken cancellationToken)
    {
        var query = await _chatbotSettingsService.SaveAsync(updateChatbotSettingsDto, cancellationToken);
        return Ok(query);
    }



}
