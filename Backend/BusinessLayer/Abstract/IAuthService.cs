using DtoLayer.AuthDtos.Items;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using SharedKernel.Enums;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken);

    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken);

    Task<string?> VerifyResetOtpAsync(string email,  string code, TwoFactorProvider provider, CancellationToken cancellationToken);
    Task<bool> SetNewPasswordAsync(string email, string newPassword,string resetToken, CancellationToken cancellationToken);






   
 
}
