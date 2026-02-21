using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    Task<bool> SendMailOtpAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> VerifyTwoFactorAsync(TwoFactorVerifyDto dto, CancellationToken cancellationToken);
    Task<Setup2FAResultDto> SetupAuthenticatorAsync(string userId, CancellationToken cancellationToken);
    Task<bool> ConfirmAuthenticatorSetupAsync(string userId, string Code, CancellationToken cancellationToken);
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);
}
