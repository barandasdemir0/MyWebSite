using DtoLayer.SkillDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
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
            TempData["Error"] = "Lütfen form alanlarını eksiksiz doldurun.";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            var query = await _skillApiService.AddAsync(createSkillDto);
            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));


    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateSkillDto updateSkillDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Lütfen form alanlarını eksiksiz doldurun.";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            var query = await _skillApiService.UpdateAsync(updateSkillDto.Id, updateSkillDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));

    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction
            (
            action: () => _skillApiService.DeleteAsync(id),
            successMessage: "Silme işlemi Başarılı oldu",
            ErrorMessage: "Silme İşlemi Başarısız oldu"
            );
    }
   
}
