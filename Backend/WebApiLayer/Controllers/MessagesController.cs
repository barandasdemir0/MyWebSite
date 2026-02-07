using BusinessLayer.Abstract;
using DtoLayer.HeroDtos;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MessagesController:ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _messageService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var query = await _messageService.GetAllAdminAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _messageService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMessageDto createMessageDto)
    {
        var query = await _messageService.AddAsync(createMessageDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    //gelen ve giden mesajlar güncellenemez
    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto updateMessageDto)
    //{
    //    updateMessageDto.Id = id;
    //    var query = await _messageService.UpdateAsync(updateMessageDto);
    //    if (query == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(query);
    //}


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _messageService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _messageService.DeleteAsync(id);
        return Ok(query);
    }
}
