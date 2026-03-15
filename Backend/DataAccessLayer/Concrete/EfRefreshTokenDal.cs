using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfRefreshTokenDal : IRefreshTokenDal
{
    private readonly AppDbContext _appDbContext;

    public EfRefreshTokenDal(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await _appDbContext.RefreshTokens.AddAsync(token, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetValidTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.RefreshTokens.FirstOrDefaultAsync
            (
                t => t.Token == token
                && t.userId == userId
                && !t.IsRevoked
                && t.ExpiresAt > DateTime.UtcNow, cancellationToken
            );
    }

    public async Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellation = default)
    {
        var tokens = await _appDbContext.RefreshTokens
            .Where(t => t.userId == userId && !t.IsRevoked).ToListAsync(cancellation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
