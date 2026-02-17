using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfNotificationDal : GenericRepository<Notification>, INotificationDal
{
    public EfNotificationDal(AppDbContext context) : base(context)
    {
    }

    public async Task<(List<Notification> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        IQueryable<Notification> query = _context.Notifications.AsNoTrackingWithIdentityResolution().IgnoreQueryFilters();
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<Notification?> ReadGetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }

    public async Task<Notification?> RestoreGetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid,cancellationToken);

    }
}
