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
        var model = new BlogCreateViewModel
        {
            topicDtos = await _topicApiService.GetAllAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateViewModel blogCreateViewModel)
    {
        if (!ModelState.IsValid)
        {
            blogCreateViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(blogCreateViewModel);

        }
        try
        {
            await _blogPostApiService.AddAsync(blogCreateViewModel.createBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "createBlogPostDto");
            blogCreateViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(blogCreateViewModel);
        }

    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var blog = await _blogPostApiService.GetByIdAsync(guid);
        if (blog== null)
        {
            return NotFound();
        }
        var model = new BlogUpdateViewModel
        {
            updateBlogPostDto = blog.Adapt<UpdateBlogPostDto>(),
            topicDtos = await _topicApiService.GetAllAsync()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BlogUpdateViewModel updateViewModel)
    {
        if (!ModelState.IsValid)
        {
            updateViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(updateViewModel);
        }
        try
        {
            var query = await _blogPostApiService.UpdateAsync(
                updateViewModel.updateBlogPostDto.Id,
                updateViewModel.updateBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "updateBlogPostDto");
            updateViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(updateViewModel);
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
