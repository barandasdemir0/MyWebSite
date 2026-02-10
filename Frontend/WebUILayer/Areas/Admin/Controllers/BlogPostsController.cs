using DtoLayer.BlogpostDtos;
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
public class BlogPostsController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;
    private readonly ITopicApiService _topicApiService;

    public BlogPostsController(IBlogPostApiService blogPostApiService, ITopicApiService topicApiService)
    {
        _blogPostApiService = blogPostApiService;
        _topicApiService = topicApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PaginationQuery query)
    {
        var pagedResult = await _blogPostApiService.GetAllAdminAsync(query);
        var model = new BlogPostIndexViewModel
        {

            blogPostDtos = pagedResult.Items,
            topicDtos = await _topicApiService.GetAllAsync(),
            CurrentPage = pagedResult.PageNumber, // BasePaginationViewModel'den geldi
            TotalPages = pagedResult.TotalPages   // BasePaginationViewModel'den geldi
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateBlogPostDto();
        model.topicList = await _topicApiService.GetAllAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogPostDto createBlogPostDto)
    {
        if (!ModelState.IsValid)
        {
            createBlogPostDto.topicList = await _topicApiService.GetAllAsync();
            return View(createBlogPostDto);

        }
        try
        {

            var query = await _blogPostApiService.AddAsync(createBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {


            ModelState.AddApiError(ex);
            createBlogPostDto.topicList = await _topicApiService.GetAllAsync();
            return View(createBlogPostDto);
        }

    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var query = await _blogPostApiService.GetByIdAsync(guid);
        query?.topicList = await _topicApiService.GetAllAsync();
        //ViewBag.TopicList = await _topicApiService.GetAllAsync();
        return View(query.Adapt<UpdateBlogPostDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateBlogPostDto updateBlogPostDto)
    {
        if (!ModelState.IsValid)
        {
            updateBlogPostDto.topicList = await _topicApiService.GetAllAsync();
            return View(updateBlogPostDto);
        }
        try
        {
            var query = await _blogPostApiService.UpdateAsync(updateBlogPostDto.Id, updateBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
            updateBlogPostDto.topicList = await _topicApiService.GetAllAsync();

            return View(updateBlogPostDto);
        }


    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        await _blogPostApiService.DeleteAsync(guid);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid guid)
    {
        await _blogPostApiService.RestoreAsync(guid);
        return RedirectToAction(nameof(Index));
    }


}
