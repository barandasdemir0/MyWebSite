using BusinessLayer.Abstract;
using DtoLayer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController: ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken)
    {
        var values = await _notificationService.GetAllAdminAsync(paginationQuery,cancellationToken);
        return Ok(values);
    }

    [HttpPut("read/{id}")]
    public async Task<IActionResult> ReadMessage(Guid id,CancellationToken cancellationToken)
    {
        var values = await _notificationService.ReadByIdAsync(id, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id,CancellationToken cancellationToken)
    {
        var values = await _notificationService.RestoreAsync(id, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
    {
        var values = await _notificationService.GetByIdAsync(id, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        await _notificationService.DeleteAsync(id,cancellationToken);
        return Ok(values);
    }
}
