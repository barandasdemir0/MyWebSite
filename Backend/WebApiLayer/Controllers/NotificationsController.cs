using BusinessLayer.Abstract;
using DtoLayer.NotificationDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]

public class NotificationsController: CrudController<NotificationDto,CreateNotificationDto,UpdateNotificationDto>
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService) : base(notificationService)
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
}
