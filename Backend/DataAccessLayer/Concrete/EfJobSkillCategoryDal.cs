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

    public async Task<JobSkillCategory?> RestoreDeleteByIdAsync(Guid guid)
    {
        return await _context.jobSkillCategories.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
    }
}
