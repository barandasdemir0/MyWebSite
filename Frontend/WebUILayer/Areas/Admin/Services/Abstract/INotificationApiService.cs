using DtoLayer.NotificationDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface INotificationApiService:IGenericApiService<NotificationDto,CreateNotificationDto,UpdateNotificationDto>
{
    Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
    Task<List<NotificationDto>> GetTopUnreadAsync(int count);
    Task ReadMessageAsync(Guid guid);
    Task RestoreAsync(Guid guid);
}
