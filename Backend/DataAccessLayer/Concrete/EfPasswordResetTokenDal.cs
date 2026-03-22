using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfPasswordResetTokenDal : IPasswordResetTokenDal
{
    private readonly AppDbContext _appDbContext;

    public EfPasswordResetTokenDal(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        await _appDbContext.PasswordResetTokens.AddAsync(token, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordResetToken?> GetValidTokenAsync(string token, string email, CancellationToken cancellationToken = default)
    {
        return await _appDbContext.PasswordResetTokens.FirstOrDefaultAsync(
            x => x.Token == token &&
            x.Email == email &&
            !x.IsUsed &&
            x.ExpiresAt > DateTime.UtcNow, cancellationToken
        );
    }

    public async Task MarkAsUsedAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        token.IsUsed = true;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
