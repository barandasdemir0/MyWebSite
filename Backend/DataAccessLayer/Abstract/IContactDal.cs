using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IContactDal:IGenericRepository<Contact>
{
    Task<Contact?> GetSingleAsync(CancellationToken cancellationToken = default);
}
