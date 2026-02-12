using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract;

public interface INotificationDal:IGenericRepository<Notification>
{
    Task<Notification?> RestoreGetByIdAsync(Guid guid,CancellationToken cancellationToken = default);
}
