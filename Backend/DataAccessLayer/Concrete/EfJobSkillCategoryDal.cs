using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfJobSkillCategoryDal : GenericRepository<JobSkillCategory>, IJobSkillCategoryDal
{
    public EfJobSkillCategoryDal(AppDbContext context) : base(context)
    {
    }

    public async Task<JobSkillCategory?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default) //silinen kaydı bul
    {
        return await _context.JobSkillCategories.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        // contextin içerisinde categornin içinde ıgnoreet neyi silinenleri gölstermemeyi sonra eşleşen ilk kaydı bul ve geri getir
    }
}
