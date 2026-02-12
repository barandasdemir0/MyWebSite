using DtoLayer.SkillDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class SkillController : Controller
{
    private readonly ISkillApiService _skillApiService;

    public SkillController(ISkillApiService skillApiService)
    {
        _skillApiService = skillApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _skillApiService.GetAllAsync();
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSkillDto createSkillDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createSkillDto);
        }
        try
        {
            var query = await _skillApiService.AddAsync(createSkillDto);
            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(createSkillDto);
        }
       
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateSkillDto updateSkillDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSkillDto);
        }
        try
        {
            var query = await _skillApiService.UpdateAsync(updateSkillDto.Id, updateSkillDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateSkillDto);
        }
      
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _skillApiService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction(nameof(Index));
    }
   
}
