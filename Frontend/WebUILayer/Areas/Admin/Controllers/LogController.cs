using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Models;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class LogController : Controller
{
    private readonly ILogApiService _logApiService;

    public LogController(ILogApiService logApiService)
    {
        _logApiService = logApiService;
    }

    public async Task<IActionResult> Index([FromQuery] PaginationQuery paginationQuery)
    {
        var pagedResult = await _logApiService.GetResultLogsAsync(paginationQuery);
        var model = new LogsIndexViewModel
        {
            ResultLogDtos = pagedResult.Items,
            CurrentPage = pagedResult.PageNumber,
            TotalPages = pagedResult.TotalPages
        };

        return View(model);
    }
}
