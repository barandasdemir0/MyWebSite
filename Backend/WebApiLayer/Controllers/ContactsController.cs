using BusinessLayer.Abstract;
using DtoLayer.ContactDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _contactService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _contactService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactDto createContactDto)
    {
        var query = await _contactService.AddAsync(createContactDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactDto updateContactDto)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _contactService.UpdateAsync(id,updateContactDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _contactService.GetByIdAsync(id);
        if (query==null)
        {
            return NotFound();
        }
        await _contactService.DeleteAsync(id);
        return Ok(query);

    }




}
