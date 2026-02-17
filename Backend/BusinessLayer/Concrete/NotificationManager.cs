using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.NotificationDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class NotificationManager : GenericManager<Notification,NotificationDto,CreateNotificationDto,UpdateNotificationDto> ,INotificationService
{
    private readonly INotificationDal _notificationDal;

    public NotificationManager(INotificationDal notificationDal, IMapper mapper) : base(notificationDal, mapper)
    {
        _notificationDal = notificationDal;
    }

    public override Task<NotificationDto> AddAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Bildirimler Eklenemez!");
    }

    public async Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, count) = await _notificationDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<NotificationDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize,count);
    }


    public async Task<NotificationDto?> ReadByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _notificationDal.ReadGetByIdAsync(guid, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsRead = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _notificationDal.UpdateAsync(entity, cancellationToken);
        await _notificationDal.SaveAsync(cancellationToken);
        return _mapper.Map<NotificationDto>(entity);
    }

    public async Task<NotificationDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _notificationDal.RestoreGetByIdAsync(guid, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _notificationDal.UpdateAsync(entity,cancellationToken);
        await _notificationDal.SaveAsync(cancellationToken);
        return _mapper.Map<NotificationDto>(entity);
    }

    public override Task<NotificationDto?> UpdateAsync(Guid guid, UpdateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Bildirimler güncellenemez!");
    }
}
