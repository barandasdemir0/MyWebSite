using DtoLayer.AuthDtos;

namespace WebUILayer.Services.Abstract;

public interface ITwoFactorApiService
{
    Task<Setup2FAResultDto?> SetupAuthenticatorAsync();
    Task<bool> ConfirmAuthenticatorAsync(TwoFactorVerifyDto dto);

    Task<bool> SendEmailCodeAsync(string userId);

    Task<LoginResultDto?> VerifyTwoFactorAsync(TwoFactorVerifyDto twoFactorVerifyDto);

}
