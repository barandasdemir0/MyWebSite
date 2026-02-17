using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfSiteSettingsDal : GenericRepository<SiteSettings>, ISiteSettingsDal
{
    public EfSiteSettingsDal(AppDbContext context) : base(context)
    {
    }

    public async Task<SiteSettings?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
}
