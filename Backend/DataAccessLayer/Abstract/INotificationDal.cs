using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface INotificationDal:IGenericRepository<Notification>
{
    Task<Notification?> RestoreGetByIdAsync(Guid guid,CancellationToken cancellationToken = default);
    Task<Notification?> ReadGetByIdAsync(Guid guid,CancellationToken cancellationToken = default);

    Task<(List<Notification> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size,CancellationToken cancellationToken=default);
}
