using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
   
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);

    Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
 
}
