using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfExperienceDal : GenericRepository<Experience>, IExperienceDal
{
    public EfExperienceDal(AppDbContext context) : base(context)
    {
    }

    public async Task<Experience?> RestoreDeleteByIdAsync(Guid guid,
CancellationToken cancellationToken = default)
    {
        return await _context.Experiences.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }
}
