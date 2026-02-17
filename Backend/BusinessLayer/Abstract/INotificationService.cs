using CV.EntityLayer.Entities;
using DtoLayer.NotificationDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface INotificationService:IGenericService<Notification,NotificationDto,CreateNotificationDto,UpdateNotificationDto>
{
    Task<NotificationDto?> ReadByIdAsync(Guid guid,CancellationToken cancellationToken = default);
    Task<NotificationDto?> RestoreAsync(Guid guid,CancellationToken cancellationToken = default);
    Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
}
