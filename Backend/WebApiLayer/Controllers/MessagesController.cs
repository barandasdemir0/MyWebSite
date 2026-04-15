using BusinessLayer.Abstract;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class MessagesController : CrudController<MessageDto,CreateMessageDto,UpdateMessageDto>
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService) : base(messageService)
    {
        _messageService = messageService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var query = await _messageService.GetAllAdmin(paginationQuery, cancellationToken);
        return Ok(query);
    }

    [Authorize(Roles = "Admin")]
    public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto updateMessageDto, CancellationToken cancellationToken)
    {
        return Ok("MESAJLAR GÜNCELLENEMEZ");
    }



    [HttpGet]
    public override async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }
        var result = await _messageService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }
        var result =  await _messageService.GetByIdAsync(id, cancellationToken);
        if (result==null)
        {
            return NotFound();
        }
        return Ok(result);
    }



}
