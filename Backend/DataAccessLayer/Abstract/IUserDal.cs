using EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IUserDal
{
    Task<List<AppUser>> GetPendingUserAsync(CancellationToken cancellationToken);
    Task<List<AppUser>> GetApprovedUserAsync(CancellationToken cancellationToken);
    Task<Dictionary<Guid, List<string>>> GetUserRolesBatchAsync(List<Guid> userIds, CancellationToken cancellationToken);
  
}
