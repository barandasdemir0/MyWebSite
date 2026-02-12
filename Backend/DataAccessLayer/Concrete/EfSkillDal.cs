using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfSkillDal : GenericRepository<Skill>, ISkillDal
    {
        public EfSkillDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Skill?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
        {
            return await _context.Skills.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        }
    }
}
