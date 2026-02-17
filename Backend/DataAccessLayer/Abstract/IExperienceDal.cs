using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IExperienceDal:IGenericRepository<Experience>
{
    Task<Experience?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);
}
