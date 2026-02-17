using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ITopicDal : IGenericRepository<Topic>
{
    Task<Topic?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);
}
