using SharedKernel.Enums;

namespace BusinessLayer.Abstract;

public interface IAccountService
{
    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken);

    Task<string?> VerifyResetOtpAsync(string email, string code, TwoFactorProvider provider, CancellationToken cancellationToken);
    Task<bool> SetNewPasswordAsync(string email, string newPassword, string resetToken, CancellationToken cancellationToken);
}
