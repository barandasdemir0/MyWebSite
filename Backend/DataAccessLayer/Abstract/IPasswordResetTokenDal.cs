using CV.EntityLayer.Entities;

namespace DataAccessLayer.Abstract;

public interface IPasswordResetTokenDal
{
    Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
    Task<PasswordResetToken?> GetValidTokenAsync(string token, string email, CancellationToken cancellationToken = default);
    Task MarkAsUsedAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
}
