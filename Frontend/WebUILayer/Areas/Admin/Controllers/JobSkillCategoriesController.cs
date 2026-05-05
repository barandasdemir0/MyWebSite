using DtoLayer.JobSkillCategoryDtos;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize]
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
       
        var query = await _jobSkillCategoryService.GetAllAdminAsync();
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
    public async Task<IActionResult> Update(Guid id)
    {
        var query = await _jobSkillCategoryService.GetByIdAsync(id);
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
        return await this.SafeAction
           (
           action: () => _jobSkillCategoryService.DeleteAsync(id),
           successMessage: "Silme işlemi Başarılı oldu",
           ErrorMessage: "Silme İşlemi Başarısız oldu"
           );
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        return await this.SafeAction
            (
            action: () => _jobSkillCategoryService.RestoreAsync(id),
            successMessage: "Geri Alma işlemi Başarılı oldu",
            ErrorMessage: "Geri Alma İşlemi Başarısız oldu"
            );
    }





}
