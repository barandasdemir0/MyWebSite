using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete;

public class EfJobSkillDal : GenericRepository<JobSkill>, IJobSkillDal
{
    public EfJobSkillDal(AppDbContext context) : base(context)
    {
    }

    public async Task<JobSkill?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
    {
        return await _context.JobSkills.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id== guid, cancellationToken);
    }
}
