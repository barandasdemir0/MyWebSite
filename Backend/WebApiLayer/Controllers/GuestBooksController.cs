using BusinessLayer.Abstract;
using DtoLayer.GithubRepoDto;
using DtoLayer.GuestBookDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class GuestBooksController:ControllerBase
{

    private readonly IGuestBookService _guestBookService;

    public GuestBooksController(IGuestBookService guestBookService)
    {
        _guestBookService = guestBookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _guestBookService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _guestBookService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestBookDto createGuestBookDto)
    {
        var query = await _guestBookService.AddAsync(createGuestBookDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    //[HttpPut("{id}")] //Güncelleme Fonksiyonu bulunmamaktadır ziyaretçi mesajları güncellenemez
    //public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGuestBookDto updateGuestBookDto)
    //{
    //    updateGuestBookDto.ıd = id;
    //    var query = await _guestBookService.UpdateAsync(updateGuestBookDto);
    //    if (query == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(query);
    //}


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _guestBookService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _guestBookService.DeleteAsync(id);
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _guestBookService.RestoreAsync(id);
        return Ok(entity);
    }
}
