using BusinessLayer.Abstract;
using DtoLayer.GithubRepoDtos;
using DtoLayer.GuestBookDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class GuestBooksController : ControllerBase
{

    private readonly IGuestBookService _guestBookService;

    public GuestBooksController(IGuestBookService guestBookService)
    {
        _guestBookService = guestBookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetAllAsync(cancellationToken);
        return Ok(query);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestBookDto createGuestBookDto, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.AddAsync(createGuestBookDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    //[HttpPut("{id}")] //Güncelleme Fonksiyonu bulunmamaktadır ziyaretçi mesajları güncellenemez
    //public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuestBookDto updateGuestBookDto, CancellationToken cancellationToken)
    //{
    //    updateGuestBookDto.ıd = id;
    //    var query = await _guestBookService.UpdateAsync(updateGuestBookDto, cancellationToken);
    //    if (query == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(query);
    //}


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _guestBookService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _guestBookService.DeleteAsync(id, cancellationToken);
        return Ok(query);
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
