using BusinessLayer.Abstract;
using DtoLayer.ContactDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]

public sealed class ContactsController : CrudController<ContactDto,CreateContactDto,UpdateContactDto>
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService) : base(contactService)
    {
        _contactService = contactService;
    }

   
    [HttpGet("single")] //upsert için işlemler
    public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
    {
        var values = await _contactService.GetSingleAsync(cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }




}
