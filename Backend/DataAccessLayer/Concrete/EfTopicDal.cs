using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfTopicDal : GenericRepository<Topic>, ITopicDal
{
    public EfTopicDal(AppDbContext context) : base(context)
    {
    }

    public async Task<Topic?> RestoreDeleteByIdAsync(Guid guid,
CancellationToken cancellationToken = default)
    {
        return await _context.Topics.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }
}
