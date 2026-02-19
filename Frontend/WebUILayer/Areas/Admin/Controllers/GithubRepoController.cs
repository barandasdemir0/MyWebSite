using DtoLayer.GithubRepoDtos;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Areas.Admin.Services.Concrete;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Controllers;


[Area("Admin")]
[Route("[area]/[controller]/[action]/{id?}")]
public class GithubRepoController : Controller
{
    private readonly IGithubApiService _githubApiService;

    public GithubRepoController(IGithubApiService githubApiService)
    {
        _githubApiService = githubApiService;
    }




    [HttpGet]
    public async Task<IActionResult> Index(string? username, [FromQuery] PaginationQuery query)
    {
        var model = new GithubIndexViewModel { Username = username ?? string.Empty };
        if (!string.IsNullOrEmpty(username))
        {
            var pagedResult = await _githubApiService.FetchFromGithubAsync(username, query);
            model.GithubRepos = pagedResult.Items;
            model.CurrentPage = pagedResult.PageNumber;
            model.TotalPages = pagedResult.TotalPages;
        }
        // Veritabanındaki kayıtlı repo isimlerini al
        var savedRepos = await _githubApiService.GetAllAsync();
        model.SavedRepoNames = savedRepos.Select(r => r.RepoName).ToList();
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Sync([FromBody] SyncGithubRequest request)
    {
        try
        {
            await _githubApiService.SyncSelectedAsync(request.Username, request.RepoNames);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> ToggleVisibility(Guid id)
    {
        return await this.SafeAction(
            action: () => _githubApiService.ToggleVisibilityAsync(id),
            successMessage: "Görünürlük değiştirildi",
            ErrorMessage: "İşlem başarısız"
        );
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await this.SafeAction(
            action: () => _githubApiService.DeleteAsync(id),
            successMessage: "Repo silindi",
            ErrorMessage: "Silme işlemi başarısız"
        );
    }


}
