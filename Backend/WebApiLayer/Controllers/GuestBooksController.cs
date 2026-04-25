using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class GuestBooksController : CrudController<GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
{

    private readonly IGuestBookService _guestBookService;

    public GuestBooksController(IGuestBookService guestBookService) : base(guestBookService)
    {
        _guestBookService = guestBookService;
    }
    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetAllAdminAsync(pagination, cancellationToken);
        return Ok(query);
    }
    [HttpGet("user-all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUser([FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetAllUserAsync(pagination, cancellationToken);
        return Ok(query);
    }

    [AllowAnonymous]
    [HttpPost]
    public override async Task<IActionResult> Create([FromBody] CreateGuestBookDto createGuestBookDto,CancellationToken cancellationToken)
    {
        var result = await _guestBookService.AddAsync(createGuestBookDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new
        {
            id = result.Id
        },result);
    }

    [Authorize(Roles =RoleConsts.Admin)]
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> Approve(Guid id,CancellationToken cancellationToken)
    {
        var entity = await _guestBookService.ApproveAsync(id, cancellationToken);
        if (entity==null)
        {
            return NotFound();
        }
        return Ok(entity);
    }







    [Authorize(Roles = RoleConsts.Admin)]

    [HttpPut("{id}")] //Güncelleme Fonksiyonu bulunmamaktadır ziyaretçi mesajları güncellenemez
    public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuestBookDto updateGuestBookDto, CancellationToken cancellationToken)
    {
        return Ok("MESAJLAR GÜNCELLENEMEZ");
    }
    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _guestBookService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

   

   

}
