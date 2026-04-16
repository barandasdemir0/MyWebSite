using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;
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

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> Create([FromBody] CreateMessageDto createDto, CancellationToken cancellationToken)
    {
        var result = await _messageService.AddAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var query = await _messageService.GetAllAdmin(paginationQuery, cancellationToken);
        return Ok(query);
    }

    [Authorize(Roles = RoleConsts.Admin)]
    public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateMessageDto updateMessageDto, CancellationToken cancellationToken)
    {
        return Ok("MESAJLAR GÜNCELLENEMEZ");
    }



    [HttpGet]
    public override async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (!User.IsInRole(RoleConsts.Admin))
        {
            return Forbid();
        }
        var result = await _messageService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!User.IsInRole(RoleConsts.Admin))
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

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("folder/{folder}")]
    public async Task<IActionResult> GetByFolder(MessageFolder folder, [FromQuery] PaginationQuery paginationQuery , CancellationToken cancellationToken)
    {
        var result = await _messageService.GetByFolderAsync(folder, paginationQuery, cancellationToken);
        return Ok(result);
    }



    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("starred")]
    public async Task<IActionResult> GetStarred([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var result = await _messageService.GetStarredAsync(paginationQuery, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("read")]

    public async Task<IActionResult> GetRead([FromQuery] PaginationQuery paginationQuery,CancellationToken cancellationToken)
    {
        var result = await _messageService.GetReadAsync(paginationQuery, cancellationToken);
        return Ok(result);
    }


    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("folder-counts")]
    public async Task<IActionResult> GetFolderCounts(CancellationToken cancellationToken)
    {
        var counts = await _messageService.GetFolderCountsAsync(cancellationToken);
        return Ok(counts);
    }


    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetDetail(Guid id,CancellationToken cancellationToken)
    {
        var result = await _messageService.GetDetailsByIdAsync(id, cancellationToken);
        if (result==null)
        {
            return NotFound();
        }
        await _messageService.MarkAsReadAsync(id, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPut("mark-read/{id}")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var result = await _messageService.MarkAsReadAsync(id, cancellationToken);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPut("toggle-star/{id}")]
    public async Task<IActionResult> ToggleStar(Guid id,CancellationToken cancellationToken)
    {
        var result = await _messageService.ToggleStarAsync(id, cancellationToken);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id,CancellationToken cancellationToken)
    {
        var entity = await _messageService.RestoreAsync(id, cancellationToken);
        if (entity==null)
        {
            return NotFound();
        }
        return Ok();
    }












}
