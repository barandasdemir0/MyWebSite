using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken);

    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken);

    Task<bool> VerifyResetOtpAsync(string email,  string code, TwoFactorProvider provider, CancellationToken cancellationToken);
    Task<bool> SetNewPasswordAsync(string email, string newPassword, CancellationToken cancellationToken);


   
 
}
