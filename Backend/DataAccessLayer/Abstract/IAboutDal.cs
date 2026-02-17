using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IAboutDal:IGenericRepository<About>
{
    Task<About?> GetSingleAsync(CancellationToken cancellationToken = default);
}
