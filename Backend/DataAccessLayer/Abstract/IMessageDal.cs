using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IMessageDal:IGenericRepository<Message>
{
    Task<(List<Message> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default);
  
}
