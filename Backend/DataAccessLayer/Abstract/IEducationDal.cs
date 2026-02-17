using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IEducationDal:IGenericRepository<Education>
{
    Task<Education?> RestoreDeleteByIdAsync(Guid guid,
    CancellationToken cancellationToken = default);
}
