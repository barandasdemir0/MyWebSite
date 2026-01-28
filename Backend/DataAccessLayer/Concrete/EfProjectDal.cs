using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfProjectDal : GenericRepository<Project>, IProjectDal
    {
        public EfProjectDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Project?> RestoreDeleteByIdAsync(Guid guid)
        {
            return await _context.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
