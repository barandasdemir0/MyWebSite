using EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface ISystemLogDal:IGenericRepository<SystemLog>
{
    Task<(List<SystemLog> items, int totalCount)> GetLogsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
