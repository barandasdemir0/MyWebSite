using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IBlogPostDal:IGenericRepository<BlogPost>
{
    Task<BlogPost?> RestoreDeletedByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);

    Task<(List<BlogPost> Items,int TotalCount)> GetAdminListPagesAsync(int page, int size,Guid? topicId,
    CancellationToken cancellationToken = default);

    Task<(List<BlogPost> Items, int TotalCount)> GetUserListPagesAsync(int page, int size,Guid? topicId, CancellationToken cancellationToken = default);
}
