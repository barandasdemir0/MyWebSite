using DtoLayer.HeroDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class HeroController : Controller
{

    private readonly IHeroApiService _heroApiService;

    public HeroController(IHeroApiService heroApiService)
    {
        _heroApiService = heroApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _heroApiService.GetHeroForEditAsync();
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateHeroDto updateHeroDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateHeroDto);
        }
        try
        {
            await _heroApiService.SaveHeroAsync(updateHeroDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateHeroDto);
        }
    }
}
