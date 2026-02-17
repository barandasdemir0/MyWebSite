using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfGithubRepoDal : GenericRepository<GithubRepo>, IGithubRepoDal
{
    public EfGithubRepoDal(AppDbContext context) : base(context)
    {
    }

    public async Task<(List<GithubRepo> Items, int TotalCount)> GetUserListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
    {
       

        IQueryable<GithubRepo> query = _context.GithubRepos.AsNoTrackingWithIdentityResolution();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items,totalCount);
    }
}
