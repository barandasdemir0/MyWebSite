using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Concrete
{
    public class EfSocialMediaDal : GenericRepository<SocialMedia>, ISocialMediaDal
    {
        public EfSocialMediaDal(AppDbContext context) : base(context)
        {
        }

        public async Task<SocialMedia?> RestoreDeleteByIdAsync(Guid guid)
        {
            return await _context.SocialMedias.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid);
        }
    }
}
