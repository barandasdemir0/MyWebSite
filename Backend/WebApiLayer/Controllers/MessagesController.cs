using BusinessLayer.Abstract;
using DtoLayer.HeroDtos;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _messageService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    //[HttpGet("admin-all")]
    //public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    //{
    //    var query = await _messageService.GetAllAdminAsync(cancellationToken);
    //    return Ok(query);
    //}
    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var query = await _messageService.GetAllAdmin(paginationQuery, cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _messageService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMessageDto createMessageDto, CancellationToken cancellationToken)
    {
        var query = await _messageService.AddAsync(createMessageDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    //gelen ve giden mesajlar güncellenemez
    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto updateMessageDto, CancellationToken cancellationToken)
    //{
    //    updateMessageDto.Id = id;
    //    var query = await _messageService.UpdateAsync(updateMessageDto, cancellationToken);
    //    if (query == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(query);
    //}


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _messageService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _messageService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }
}
