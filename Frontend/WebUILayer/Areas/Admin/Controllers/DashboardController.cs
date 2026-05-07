using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class DashboardController : Controller
{
    private readonly IBlogPostApiService _blogPostApiService;
    private readonly IProjectApiService _projectApiService;
    private readonly IGuestBookApiService _guestBookApiService;
    private readonly IMessageApiService _messageApiService;

    public DashboardController(IBlogPostApiService blogPostApiService, IProjectApiService projectApiService, IGuestBookApiService guestBookApiService, IMessageApiService messageApiService)
    {
        _blogPostApiService = blogPostApiService;
        _projectApiService = projectApiService;
        _guestBookApiService = guestBookApiService;
        _messageApiService = messageApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogsList = await _blogPostApiService.GetLatestAsync(3);
        var projectsList = await _projectApiService.GetLatestAsync(3);
        var guestBookList = await _guestBookApiService.GetLatestAsync(3);
        var messagesList = await _messageApiService.GetLatestAsync(3);
        var model = new DashboardIndexViewModel
        {
            blogPostListDtos = blogsList,
            projectListDtos = projectsList,
            guestBookListDtos = guestBookList,
            messageListDtos = messagesList
        };

        return View(model);
    }
}
