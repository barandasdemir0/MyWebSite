using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete;

public class EfNotificationDal : GenericRepository<Notification>, INotificationDal
{
    public EfNotificationDal(AppDbContext context) : base(context)
    {
    }

    public async Task<Notification?> RestoreGetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid,cancellationToken);

    }
}
