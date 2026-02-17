using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class ProjectController : Controller
{
    private readonly IProjectApiService _projectApiService;
    private readonly ITopicApiService _topicApiService;

    public ProjectController(IProjectApiService projectApiService, ITopicApiService topicApiService)
    {
        _projectApiService = projectApiService;
        _topicApiService = topicApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {
        var pagedResult = await _projectApiService.GetAllAdminAsync(paginationQuery);
        var model = new ProjectIndexViewModel
        {
            projectDtos = pagedResult.Items,
            topicDtos = await _topicApiService.GetAllAsync(),
            CurrentPage = pagedResult.PageNumber,
            TotalPages = pagedResult.TotalPages
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ProjectCreateViewModel
        {
            topicDtos = await _topicApiService.GetAllAsync()
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProjectCreateViewModel createViewModel)
    {
        if (!ModelState.IsValid)
        {
            createViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(createViewModel);
        }
        try
        {
            await _projectApiService.AddAsync(createViewModel.CreateProjectDtos);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "CreateProjectDtos");
            createViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(createViewModel);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var project = await _projectApiService.GetByIdAsync(guid);
        if (project == null)
        {
            return NotFound();
        }
        var model = new ProjectUpdateViewModel
        {
            UpdateProjectDto = project.Adapt<UpdateProjectDto>(),
            topicDtos =  await _topicApiService.GetAllAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProjectUpdateViewModel projectUpdateView)
    {
        if (!ModelState.IsValid)
        {
            projectUpdateView.topicDtos = await _topicApiService.GetAllAsync();
            return View(projectUpdateView);
        }
        try
        {
            var query = await _projectApiService.UpdateAsync(
                projectUpdateView.UpdateProjectDto.Id, 
                projectUpdateView.UpdateProjectDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "UpdateProjectDto");
            projectUpdateView.topicDtos = await _topicApiService.GetAllAsync();
            return View(projectUpdateView);
        }
    }



    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction
            (
            action: () => _projectApiService.DeleteAsync(id),
            successMessage: "Silme işlemi Başarılı oldu",
            ErrorMessage: "Silme İşlemi Başarısız oldu"
            );
    }
    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        return await this.SafeAction
            (
            action: () => _projectApiService.RestoreAsync(id),
            successMessage: "Geri Alma işlemi Başarılı oldu",
            ErrorMessage: "Geri Alma İşlemi Başarısız oldu"
            );
    }





}
