using DtoLayer.AuthDtos.Items;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using SharedKernel.Enums;

namespace BusinessLayer.Abstract;

public interface IAuthService
{
    Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
    
    
}
