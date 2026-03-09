using DtoLayer.AuthDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IAuthApiService
{
    Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
    Task<RegisterResultDto?> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();




    Task<UserProfileDto?> GetUserProfileAsync(string userId);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    Task<bool> Toggle2FAAsync(string userId, Toggle2FADto dto);




    Task<Setup2FAResultDto?> SetupAuthenticatorAsync(string userId);
    Task<bool> ConfirmAuthenticatorAsync(TwoFactorVerifyDto dto);

    Task<bool> SendEmailCodeAsync(string userId);

    Task<LoginResultDto?> VerifyTwoFactorAsync(TwoFactorVerifyDto twoFactorVerifyDto);


   
  


}
