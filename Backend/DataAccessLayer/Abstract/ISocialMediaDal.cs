using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ISocialMediaDal:IGenericRepository<SocialMedia>
{
    Task<SocialMedia?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);
}
