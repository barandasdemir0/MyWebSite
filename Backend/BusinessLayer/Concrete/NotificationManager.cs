using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using DataAccessLayer.Abstract;
using DtoLayer.NotificationDtos;
using EntityLayer.Concrete;
using MapsterMapper;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete;

public class NotificationManager : INotificationService
{
    private readonly INotificationDal _notificationDal;
    private readonly IMapper _mapper;

    public NotificationManager(INotificationDal notificationDal, IMapper mapper)
    {
        _notificationDal = notificationDal;
        _mapper = mapper;
    }

    public Task<NotificationDto> AddAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var query = await _notificationDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
        if (query!=null)
        {
            await _notificationDal.DeleteAsync(query, cancellationToken);
            await _notificationDal.SaveAsync(cancellationToken);
        }
    }

    public async Task<PagedResult<NotificationDto>> GetAllAdminAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, count) = await _notificationDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<NotificationDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize,count);
    }

    public async Task<List<NotificationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _notificationDal.GetAllAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<NotificationDto>>(entity);
    }

    public async Task<NotificationDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _notificationDal.GetByIdAsync(guid, cancellationToken:cancellationToken);
        if (entity==null)
        {
            return null;
        }
        return _mapper.Map<NotificationDto>(entity);
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

    public Task<NotificationDto?> UpdateAsync(Guid guid, UpdateNotificationDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
