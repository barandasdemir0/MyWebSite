using DtoLayer.AboutDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]


public class AboutsController : Controller
{
    private readonly IAboutApiService _aboutApiService;

    public AboutsController(IAboutApiService aboutApiService)
    {
        _aboutApiService = aboutApiService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
       
        var query = await _aboutApiService.GetAboutForEditAsync();
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateAboutDto updateAboutDto)
    {
        if (!ModelState.IsValid) return View(updateAboutDto);
        await _aboutApiService.SaveAboutAsync(updateAboutDto);
        return RedirectToAction(nameof(Index));

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _aboutApiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }



}
