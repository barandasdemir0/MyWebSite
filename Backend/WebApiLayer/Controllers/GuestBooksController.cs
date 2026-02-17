using BusinessLayer.Abstract;
using DtoLayer.GuestBookDtos;
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
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetAllAdminAsync(pagination, cancellationToken);
        return Ok(query);
    }
    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllUser([FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetAllUserAsync(pagination, cancellationToken);
        return Ok(query);
    }



    [HttpPut("{id}")] //Güncelleme Fonksiyonu bulunmamaktadır ziyaretçi mesajları güncellenemez
    public override async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuestBookDto updateGuestBookDto, CancellationToken cancellationToken)
    {
        return Ok("MESAJLAR GÜNCELLENEMEZ");
    }

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
