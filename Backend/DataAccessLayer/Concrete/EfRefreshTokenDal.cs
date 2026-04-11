using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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

    public async Task<RefreshToken?> GetValidTokenAsync(string token, Guid userId, CancellationToken cancellationToken = default) //geçerli tokeni alma 
    {
        return await _appDbContext.RefreshTokens.FirstOrDefaultAsync //ilk tokeni döndür
            (
                t => t.Token == token // tokenler eşleşmeli
                && t.UserId == userId //kullanıcı esleşmeli
                && !t.IsRevoked //iptal olmamalıu
                && t.ExpiresAt > DateTime.UtcNow, cancellationToken //ve dolma süresi bugünden büyük olmalı
            );
    }

    public async Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellation = default)//kullanıcıya ait tüm tokenleri iptal etme
    {
        var tokens = await _appDbContext.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked).ToListAsync(cancellation); // user elkelşyorsa ve iptal olmamışsa
        foreach (var token in tokens) // tokenleri çek
        {
            token.IsRevoked = true; // bütün eski tokenleri iptal et
        }
        await _appDbContext.SaveChangesAsync(cancellation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
