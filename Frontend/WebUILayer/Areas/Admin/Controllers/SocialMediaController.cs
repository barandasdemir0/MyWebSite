using DtoLayer.SocialMediaDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class SocialMediaController : Controller
{

    private readonly ISocialMediaApiService _socialMediaApiService;

    public SocialMediaController(ISocialMediaApiService socialMediaApiService)
    {
        _socialMediaApiService = socialMediaApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _socialMediaApiService.GetAdminAllAsync();
        return View(query);
    }

    [HttpGet]
    public IActionResult Create() => View();


    [HttpPost]
    public async Task<IActionResult> Create(CreateSocialMediaDto createSocialMediaDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createSocialMediaDto);
        }
        try
        {
            await _socialMediaApiService.AddAsync(createSocialMediaDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(createSocialMediaDto);
        }
       
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var query = await _socialMediaApiService.GetByIdAsync(guid);
        return View(query.Adapt<UpdateSocialMediaDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateSocialMediaDto updateSocialMediaDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSocialMediaDto);
        }
        try
        {
            await _socialMediaApiService.UpdateAsync(updateSocialMediaDto.Id, updateSocialMediaDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateSocialMediaDto);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        await _socialMediaApiService.DeleteAsync(guid);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid guid)
    {
        await _socialMediaApiService.RestoreAsync(guid);
        return RedirectToAction(nameof(Index));
    }



}
