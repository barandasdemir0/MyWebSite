using DtoLayer.GuestBookDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using System.Text.Json;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Models;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Controllers;

public class GuestBookController : Controller
{
    private readonly IGuestBookApiService _guestBookApiService;
    private readonly IGuestSessionService _guestSessionService;

    public GuestBookController(IGuestBookApiService guestBookApiService, IGuestSessionService guestSessionService)
    {
        _guestBookApiService = guestBookApiService;
        _guestSessionService = guestSessionService;
    }

    public const string name = "GuestBook";

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {
        try
        {
            var viewModel = new GuestBookViewModel
            {
                Messages = await _guestBookApiService.GetAllUserAsync(paginationQuery),
                GuestUser = _guestSessionService.GetCurrentGuest() // Veriyi temizce aldık
            };
            return View(viewModel); // ViewModel'i HTML'e yolladık
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Ziyaretçi defteri mesajları şu an yüklenemiyor.";
            return RedirectToAction("Index", "Default");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostMessage(string message)
    {
        var guestUser = _guestSessionService.GetCurrentGuest(); // Tertemiz çağırdık
        if (guestUser == null)
        {
            return RedirectToAction(nameof(Index));
        }
        if (string.IsNullOrWhiteSpace(message))
        {
            TempData["ErrorMessage"] = "Lütfen geçerli bir mesaj yazın.";
            return RedirectToAction(nameof(Index));
        }
        guestUser.Message = message;
        try
        {
            await _guestBookApiService.AddAsync(guestUser);
            TempData["SuccessMessage"] = "Mesajınız alındı, moderasyon sonrası yayınlanacaktır.";
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Gönderim sırasında sunucu kaynaklı bir hata oluştu.";
        }
        return RedirectToAction(nameof(Index));
    }

    

}
