using DtoLayer.GuestBookDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using System.Text.Json;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Controllers;

public class GuestBookController : Controller
{
    private readonly IGuestBookApiService _guestBookApiService;

    public GuestBookController(IGuestBookApiService guestBookApiService)
    {
        _guestBookApiService = guestBookApiService;
    }

    public const string name = "GuestBook";

    [HttpGet]
    public  async Task< IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {

        var messages = await _guestBookApiService.GetAllUserAsync(paginationQuery);
        var guestUserJson = HttpContext.Session.GetString("GuestUser");
        if (!string.IsNullOrEmpty(guestUserJson))
        {
            ViewBag.GuestUser = JsonSerializer.Deserialize<CreateGuestBookDto>(guestUserJson);
        }
        return View(messages);
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(string message)
    {
        var guestUserJson = HttpContext.Session.GetString("GuestUser");
        if (string.IsNullOrEmpty(guestUserJson))
        {
            return RedirectToAction(nameof(Index));
        }
        var dto = JsonSerializer.Deserialize<CreateGuestBookDto>(guestUserJson);
        dto!.Message = message;

        try
        {
            await _guestBookApiService.AddAsync(dto);
            TempData["SuccessMessage"] = "Mesajınız alındı, moderasyon sonrası yayınlanacaktır.";
        }
        catch (Exception)
        {

            TempData["ErrorMessage"] = "Gönderim hatası.";
        }

        return RedirectToAction(nameof(Index));



    }



}
