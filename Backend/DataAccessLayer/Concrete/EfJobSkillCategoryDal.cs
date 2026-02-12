using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete;

public class EfJobSkillCategoryDal : GenericRepository<JobSkillCategory>, IJobSkillCategoryDal
{
    public EfJobSkillCategoryDal(AppDbContext context) : base(context)
    {
    }

    public async Task<JobSkillCategory?> RestoreDeleteByIdAsync(Guid guid) //silinen kaydı bul
    {
        return await _context.JobSkillCategories.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        // contextin içerisinde categornin içinde ıgnoreet neyi silinenleri gölstermemeyi sonra eşleşen ilk kaydı bul ve geri getir
    }
}
