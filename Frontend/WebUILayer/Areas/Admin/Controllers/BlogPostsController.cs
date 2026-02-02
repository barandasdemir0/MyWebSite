using DtoLayer.BlogpostDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class BlogPostsController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;

    public BlogPostsController(IBlogPostApiService blogPostApiService)
    {
        _blogPostApiService = blogPostApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = await _blogPostApiService.GetAllAdminAsync();

        return View(query);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogPostDto createBlogPostDto)
    {
        if (!ModelState.IsValid)
        {
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
            return View(createBlogPostDto);
        }
      
    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid guid)
    {
        var query = await _blogPostApiService.GetByIdAsync(guid);
        return View(query);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Guid guid, UpdateBlogPostDto updateBlogPostDto)
    {
        if (!ModelState.IsValid)
        {
            return View(updateBlogPostDto);
        }
        try
        {
            var query = await _blogPostApiService.UpdateAsync(guid, updateBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex);
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
