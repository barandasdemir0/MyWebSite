using DtoLayer.JobSkillCategoryDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class JobSkillCategoriesController : Controller
{
    private readonly IJobSkillCategoryService _jobSkillCategoryService;
    private readonly IJobSkillApiService _jobSkillApiService;

    public JobSkillCategoriesController(IJobSkillCategoryService jobSkillCategoryService, IJobSkillApiService jobSkillApiService)
    {
        _jobSkillCategoryService = jobSkillCategoryService;
        _jobSkillApiService = jobSkillApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        //var model = new JobSkillIndexViewModel
        //{
        //    jobSkillDtos = await _jobSkillApiService.GetAllAdminAsync(),
        //    jobSkillCategoryDtos = await _jobSkillCategoryService.GetAdminAllAsync()
        //};
        var query = await _jobSkillCategoryService.GetAdminAllAsync();
        return View(query);
    }

    [HttpGet]
    public IActionResult Create() => View();
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobSkillCategoryDto createJobSkillCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createJobSkillCategoryDto);
        }
        try
        {
            var model = await _jobSkillCategoryService.AddAsync(createJobSkillCategoryDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(createJobSkillCategoryDto);
        }

    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var query = await _jobSkillCategoryService.GetByIdAsync(guid);
        if (query == null)
        {
            return NotFound();
        }
        return View(query.Adapt<UpdateJobSkillCategoryDto>());
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateJobSkillCategoryDto updateJobSkillCategoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateJobSkillCategoryDto);
        }
        try
        {
            var model = await _jobSkillCategoryService.UpdateAsync(updateJobSkillCategoryDto.Id, updateJobSkillCategoryDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateJobSkillCategoryDto);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _jobSkillCategoryService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            await _jobSkillCategoryService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }

        return RedirectToAction(nameof(Index));
    }





}
