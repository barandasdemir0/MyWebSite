using BusinessLayer.Abstract;
using EntityLayer.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConsts.Admin)]
public class LogsController : ControllerBase
{
    private readonly ISystemLogService _systemLogService;

    public LogsController(ISystemLogService systemLogService)
    {
        _systemLogService = systemLogService;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellation)
    {
        var values = await _systemLogService.GetAllPagedAsync(paginationQuery,cancellation);
        return Ok(values);
    }
}
