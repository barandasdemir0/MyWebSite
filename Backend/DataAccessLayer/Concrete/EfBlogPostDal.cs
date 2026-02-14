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

        public async Task<(List<BlogPost> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size,Guid? topicId,
    CancellationToken cancellationToken = default)
        {
            IQueryable<BlogPost> query = _context.BlogPosts.AsNoTrackingWithIdentityResolution().IgnoreQueryFilters().Include(x => x.BlogTopics).ThenInclude(y => y.Topic);

            if (topicId.HasValue) // bu işlem topicId != null ile aynı işlemdir
            {
                query = query.Where(x => x.BlogTopics.Any(y => y.TopicId == topicId.Value));
            }



            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<(List<BlogPost> Items, int TotalCount)> GetUserListPagesAsync(int page, int size,Guid? topicId, CancellationToken cancellationToken = default)
        {
            IQueryable<BlogPost> query = _context.BlogPosts.AsNoTrackingWithIdentityResolution().Include(x=>x.BlogTopics).ThenInclude(y=>y.Topic);

            if (topicId.HasValue)
            {
                query = query.Where(x => x.BlogTopics.Any(y => y.TopicId == topicId.Value));
            }
         

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (items, totalCount);


        }

        public async Task<BlogPost?> RestoreDeletedByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
        {
            return await _context.BlogPosts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        }
    }
}
