using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface ITwoFactorService
{
    Task<bool> SendMailOtpAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> VerifyTwoFactorAsync(TwoFactorVerifyDto dto, CancellationToken cancellationToken);
    Task<Setup2FAResultDto> SetupAuthenticatorAsync(string userId, CancellationToken cancellationToken);
    Task<bool> ConfirmAuthenticatorSetupAsync(string userId, string Code, CancellationToken cancellationToken);

    Task<bool> Toggle2FAAsync(string userId, Toggle2FADto toggle2FADto, CancellationToken cancellationToken);
}
