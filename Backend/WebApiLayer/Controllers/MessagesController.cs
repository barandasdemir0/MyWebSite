using BusinessLayer.Abstract;
using DtoLayer.MessageDtos;
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

    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var query = await _messageService.GetAllAdmin(paginationQuery, cancellationToken);
        return Ok(query);
    }
 
    public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto updateMessageDto, CancellationToken cancellationToken)
    {
        return Ok("MESAJLAR GÜNCELLENEMEZ");
    }



}
