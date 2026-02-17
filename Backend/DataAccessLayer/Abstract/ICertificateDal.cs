using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ICertificateDal:IGenericRepository<Certificate>
{
    Task<Certificate?> RestoreDeleteByIdAsync(Guid guid, CancellationToken cancellationToken = default);
}
