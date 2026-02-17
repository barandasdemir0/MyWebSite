using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class EfProjectDal : GenericRepository<Project>, IProjectDal
    {
        public EfProjectDal(AppDbContext context) : base(context)
        {
        }
        public async Task<(List<Project> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, Guid? topicId,
    CancellationToken cancellationToken = default)
        {
            ////AsNoTracking, aynı Id’ye sahip entity’leri bellekte birden fazla nesne olarak oluşturabilir.
            //AsNoTrackingWithIdentityResolution ise tracking kapalıyken bile aynı entity’nin tek bir instance olarak kullanılmasını sağlar ve ilişkili verileri koleksiyonlar altında toplar.
            IQueryable<Project> query = _context.Projects.AsNoTrackingWithIdentityResolution().IgnoreQueryFilters().Include(x => x.ProjectTopics).ThenInclude(y => y.Topic);
            //neden IQueryable kullandık çünkü sorguya eklemeler yaparız ve en son bittiği zaman sqle çevirir
            if (topicId.HasValue)
            {
                query = query.Where(x => x.ProjectTopics.Any(y => y.TopicId == topicId.Value));
            }
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return (items, totalCount);
        }

        public async Task<(List<Project> Items, int TotalCount)> GetUserListPagesAsync(int page, int size,Guid? topicId ,CancellationToken cancellationToken = default)
        {
            IQueryable<Project> query = _context.Projects.AsNoTrackingWithIdentityResolution();
            if (topicId.HasValue)
            {
                query = query.Where(x => x.ProjectTopics.Any(y => y.TopicId == topicId));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Project?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default)
        {
            return await _context.Projects.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
        }
    }
}
