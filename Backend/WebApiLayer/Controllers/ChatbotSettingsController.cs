using BusinessLayer.Abstract;
using DtoLayer.ChatbotSettingsDtos;
using DtoLayer.HeroDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatbotSettingsController : ControllerBase
{

    private readonly IChatbotSettingsService _chatbotSettingsService;

    public ChatbotSettingsController(IChatbotSettingsService chatbotSettingsService)
    {
        _chatbotSettingsService = chatbotSettingsService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _chatbotSettingsService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _chatbotSettingsService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChatbotSettingsDto createChatbotSettingsDto, CancellationToken cancellationToken)
    {
        var query = await _chatbotSettingsService.AddAsync(createChatbotSettingsDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChatbotSettingsDto updateChatbotSettingsDto, CancellationToken cancellationToken)
    {
     
        var query = await _chatbotSettingsService.UpdateAsync(id, updateChatbotSettingsDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _chatbotSettingsService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _chatbotSettingsService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }
}
