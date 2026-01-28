using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfEducationDal : GenericRepository<Education>, IEducationDal
    {
        public EfEducationDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Education?> RestoreDeleteByIdAsync(Guid guid)
        {
            return await _context.Educations.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
