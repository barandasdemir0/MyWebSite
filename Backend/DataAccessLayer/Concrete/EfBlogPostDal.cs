using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfBlogPostDal : GenericRepository<BlogPost>, IBlogPostDal
    {
        public EfBlogPostDal(AppDbContext context) : base(context)
        {
        }

        public async Task<BlogPost?> RestoreDeletedByIdAsync(Guid guid)
        {
            return await _context.BlogPosts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
