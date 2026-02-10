using DtoLayer.BlogpostDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.Shared;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
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
        var model = new CreateProjectDto();
        model.topicList = await _topicApiService.GetAllAsync();
        // sırf topiclisti gönderebilmek adına  newledik
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid)
        {
            createProjectDto.topicList = await _topicApiService.GetAllAsync();
            return View(createProjectDto);
        }
        try
        {
            await _projectApiService.AddAsync(createProjectDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            createProjectDto.topicList = await _topicApiService.GetAllAsync();
            return View(createProjectDto);
        }
    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var query = await _projectApiService.GetByIdAsync(guid);
        query?.topicList = await _topicApiService.GetAllAsync();
        return View(query.Adapt<UpdateProjectDto>());
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProjectDto updateProjectDto)
    {
        if (!ModelState.IsValid)
        {
            updateProjectDto.topicList = await _topicApiService.GetAllAsync();
            return View(updateProjectDto);
        }
        try
        {
            var query = await _projectApiService.UpdateAsync(updateProjectDto.Id, updateProjectDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            updateProjectDto.topicList = await _topicApiService.GetAllAsync();
            return View(updateProjectDto);
        }
    }



    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        await _projectApiService.DeleteAsync(guid);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid guid)
    {
        await _projectApiService.RestoreAsync(guid);
        return RedirectToAction(nameof(Index));
    }





}
