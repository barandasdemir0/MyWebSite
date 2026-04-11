using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;

namespace WebUILayer.Services.Abstract;

public interface IAuthApiService
{
    Task<LoginResultDto?> LoginAsync(LoginDto loginDto);
    Task<RegisterResultDto?> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();
}
