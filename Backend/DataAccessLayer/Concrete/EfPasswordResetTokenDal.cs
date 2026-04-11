using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
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
        return await _appDbContext.PasswordResetTokens.FirstOrDefaultAsync( //tabloya eriş 
            x => x.Token == token && // girilen token ile veritabnındaki tokeni eşleştir 
            x.Email == email && //token kullanıcıya ait olmalı
            !x.IsUsed && // token daha önce kullanılmamış olmalı
            x.ExpiresAt > DateTime.UtcNow, cancellationToken //token henüz geçerli  süresi dolmamış olmalı
        );
    }

    //bu method parola sıfırlam tokeni kullanıldıktan sonra işaretlemek için kullanılır
    public async Task MarkAsUsedAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        token.IsUsed = true;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
