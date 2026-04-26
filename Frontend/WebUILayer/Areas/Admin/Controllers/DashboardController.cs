using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
[Route("[area]/[controller]/[action]/{id?}")]
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
       
        var model = new DashboardIndexViewModel
        {
            blogPostListDtos = await _blogPostApiService.GetLatestAsync(3), //3 tanesini getir diyoruz
            projectListDtos = await _projectApiService.GetLatestAsync(3),
            guestBookListDtos = await _guestBookApiService.GetLatestAsync(3),
            messageListDtos = await _messageApiService.GetLatestAsync(3),
          
           
        };
        return View(model);
    }
}
