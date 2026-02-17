using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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

        public async Task<SocialMedia?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
        {
            return await _context.SocialMedias.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        }
    }
}
