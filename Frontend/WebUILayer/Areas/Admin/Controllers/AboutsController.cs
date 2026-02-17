using DtoLayer.AboutDtos;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
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
        if (!ModelState.IsValid)
        {
            return View(updateAboutDto);
        }

        try
        {
            await _aboutApiService.SaveAboutAsync(updateAboutDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {


            ModelState.AddApiError(ex);
            return View(updateAboutDto);
      
        }
      
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _aboutApiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }



}
