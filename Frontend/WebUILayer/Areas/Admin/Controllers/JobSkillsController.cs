using DtoLayer.JobSkillCategoryDtos;
using DtoLayer.JobSkillsDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class JobSkillsController : Controller
{
    private readonly IJobSkillApiService _jobSkillApiService;
    private readonly IJobSkillCategoryService _jobSkillCategoryService;

    public JobSkillsController(IJobSkillApiService jobSkillApiService, IJobSkillCategoryService jobSkillCategoryService)
    {
        _jobSkillApiService = jobSkillApiService;
        _jobSkillCategoryService = jobSkillCategoryService;
    }

   

    [HttpGet]
    public async Task<IActionResult> Create(Guid id)
    {
        var category = await _jobSkillCategoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        var model = new JobSkillCreateViewModel
        {
            jobSkillCategoryDtos = category,
            jobSkillDtos = new CreateJobSkillDto
            {
                JobSkillCategoryId = id
            }
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(JobSkillCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.jobSkillCategoryDtos = await _jobSkillCategoryService.GetByIdAsync(model.jobSkillDtos.JobSkillCategoryId);
            return View(model);
        }
        try
        {
            await _jobSkillApiService.AddAsync(model.jobSkillDtos);
            return RedirectToAction("Index", "JobSkillCategories", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "jobSkillDtos");
            model.jobSkillCategoryDtos = await _jobSkillCategoryService.GetByIdAsync(model.jobSkillDtos.JobSkillCategoryId);
            return View(model);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var skill = await _jobSkillApiService.GetByIdAsync(id);
        if (skill== null)
        {
            return NotFound();
        }
        var category = await _jobSkillCategoryService.GetByIdAsync(skill.JobSkillCategoryId);
        var model = new JobSkillUpdateViewModel
        {
            jobSkillCategoryDtos = category,
            jobSkillDtos = skill.Adapt<UpdateJobSkillDto>()
           
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Update(JobSkillUpdateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.jobSkillCategoryDtos = await _jobSkillCategoryService.GetByIdAsync(model.jobSkillDtos.JobSkillCategoryId);
            return View(model);
        }
        try
        {
            await _jobSkillApiService.UpdateAsync(model.jobSkillDtos.Id,model.jobSkillDtos);
            return RedirectToAction("Index", "JobSkillCategories", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "jobSkillDtos");
            model.jobSkillCategoryDtos = await _jobSkillCategoryService.GetByIdAsync(model.jobSkillDtos.JobSkillCategoryId);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _jobSkillApiService.DeleteAsync(id);
            TempData["Success"] = "Silme işlemi başarılı.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Silme işlemi başarısız oldu.";
        }
        return RedirectToAction("Index", "JobSkillCategories", new { area = "Admin" });
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            await _jobSkillApiService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }
        
        return RedirectToAction("Index", "JobSkillCategories", new { area = "Admin" });
    }



}
