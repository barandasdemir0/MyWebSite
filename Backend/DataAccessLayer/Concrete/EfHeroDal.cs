using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfHeroDal : GenericRepository<Hero>, IHeroDal
{
    public EfHeroDal(AppDbContext context) : base(context)
    {
    }

    public async Task<Hero?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Heroes.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
}
