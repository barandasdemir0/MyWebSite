using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfExperienceDal : GenericRepository<Experience>, IExperienceDal
    {
        public EfExperienceDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Experience?> RestoreDeleteByIdAsync(Guid guid)
        {
            return await _context.Experiences.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
