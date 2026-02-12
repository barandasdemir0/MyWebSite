using DtoLayer.BlogpostDtos;
using DtoLayer.Shared;
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
            topicDtos = await _topicApiService.GetAllAsync() // Dropdown için topic listesi
                                                             // createBlogPostDto boş kalıyor — form boş açılacak
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateViewModel blogCreateViewModel)
    {
        if (!ModelState.IsValid)
        {
            // Topic listesi form POST'unda GELMİYOR (sadece seçilen id gelir)
            // Bu yüzden tekrar API'den çekerek eklememiz lazım
            blogCreateViewModel.topicDtos = await _topicApiService.GetAllAsync();
            return View(blogCreateViewModel);

        }
        try
        {
            // ViewModel'in İÇİNDEKİ DTO'yu gönder, ViewModel'in kendisini DEĞİL
            await _blogPostApiService.AddAsync(blogCreateViewModel.createBlogPostDto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddApiError(ex, "createBlogPostDto");// prefix belirtiliyor
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
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _blogPostApiService.DeleteAsync(id);
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
            await _blogPostApiService.RestoreAsync(id);
            TempData["Success"] = "Geri yükleme işlemi başarılı oldu.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Geri yükleme işlemi başarısız oldu.";
        }

        return RedirectToAction(nameof(Index));
    }



   


}
