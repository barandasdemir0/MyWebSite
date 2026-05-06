using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfSystemLogDal : GenericRepository<SystemLog>, ISystemLogDal
{
    public EfSystemLogDal(AppDbContext context) : base(context)
    {
    }

    public async Task<(List<SystemLog> items, int totalCount)> GetLogsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Logs.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }
}
