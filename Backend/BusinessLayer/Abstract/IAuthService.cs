using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken);

   
 
}
