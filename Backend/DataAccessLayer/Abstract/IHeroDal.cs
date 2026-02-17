using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IHeroDal:IGenericRepository<Hero>
{
    Task<Hero?> GetSingleAsync(CancellationToken cancellationToken = default); 
}
