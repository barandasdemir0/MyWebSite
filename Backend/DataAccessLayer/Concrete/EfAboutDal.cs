using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfAboutDal : GenericRepository<About>, IAboutDal
    {
        public EfAboutDal(AppDbContext context) : base(context)
        {
        }

        public async Task<About?> GetSingleAsync(CancellationToken cancellationToken = default)
        {
            return await _context.About.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }
    }
}
