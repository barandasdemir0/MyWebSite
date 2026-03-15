using EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IRefreshTokenDal
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetValidTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default);
    Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
