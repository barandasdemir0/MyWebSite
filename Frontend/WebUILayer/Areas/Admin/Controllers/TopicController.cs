using DtoLayer.TopicDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class TopicController : Controller
{
    private readonly ITopicApiService _topicApiService;

    public TopicController(ITopicApiService topicApiService)
    {
        _topicApiService = topicApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _topicApiService.GetAllAdminAsync();
        return View(query);
    }

    [HttpGet]
    public IActionResult Create() => View();


    [HttpPost]
    public async Task<IActionResult> Create(CreateTopicDto createTopicDto)
    {
        var query = await _topicApiService.AddAsync(createTopicDto);
        return RedirectToAction(nameof(Index));

    }
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var query = await _topicApiService.GetByIdAsync(id);
        return View(query);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Guid id, UpdateTopicDto updateTopicDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateTopicDto);
        }
        try
        {
            var query = await _topicApiService.UpdateAsync(id, updateTopicDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            return View(updateTopicDto);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        await _topicApiService.DeleteAsync(guid);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid guid)
    {
        await _topicApiService.RestoreAsync(guid);
        return RedirectToAction(nameof(Index));
    }


}
