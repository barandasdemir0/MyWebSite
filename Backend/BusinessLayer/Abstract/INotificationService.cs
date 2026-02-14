using DtoLayer.NotificationDtos;
using EntityLayer.Concrete;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface INotificationService:IGenericService<Notification,NotificationDto,CreateNotificationDto,UpdateNotificationDto>
{
    Task<NotificationDto?> ReadByIdAsync(Guid guid,CancellationToken cancellationToken = default);
    Task<NotificationDto?> RestoreAsync(Guid guid,CancellationToken cancellationToken = default);
    Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
}
