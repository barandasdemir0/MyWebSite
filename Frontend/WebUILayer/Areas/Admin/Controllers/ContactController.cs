using DtoLayer.ContactDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class ContactController : Controller
{
    private readonly IContactApiService _contactApiService;

    public ContactController(IContactApiService contactApiService)
    {
        _contactApiService = contactApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _contactApiService.GetContactForEditAsync();
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateContactDto updateContactDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateContactDto);
        }
        try
        {
            await _contactApiService.SaveContactAsync(updateContactDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateContactDto);
        }
    }
}
