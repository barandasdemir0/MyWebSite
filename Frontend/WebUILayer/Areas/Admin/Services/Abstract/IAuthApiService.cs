using DtoLayer.AuthDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IAuthApiService
{
    Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
    Task<RegisterResultDto?> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();

    Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<bool> VerifyResetOtpAsync(VerifyResetOtpDto verifyResetOtpDto);
    Task<bool> SetNewPasswordAsync(SetNewPasswordDto setNewPasswordDto);

}
