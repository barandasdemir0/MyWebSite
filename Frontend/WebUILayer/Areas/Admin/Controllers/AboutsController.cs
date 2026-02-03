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
        //var query = await _aboutApiService.GetAllAsync();
        //return View(query);
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

    #region eski güncelleme ve update komutları yukarııda tek komutta yapıldı
    //[HttpGet]
    //public IActionResult Create() => View();

    //[HttpPost]
    //public async Task<IActionResult> Create(CreateAboutDto createAboutDto)
    //{
    //    var query = await _aboutApiService.AddAsync(createAboutDto);
    //    return RedirectToAction(nameof(Index));
    //}

    //[HttpGet]
    //public async Task<IActionResult> Update(Guid id)
    //{
    //    var query = await _aboutApiService.GetByIdAsync(id);
    //    return View(query);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Update(UpdateAboutDto updateAboutDto)
    //{
    //    await _aboutApiService.UpdateAsync(updateAboutDto);
    //    return RedirectToAction(nameof(Index));
    //}

    #endregion

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _aboutApiService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
