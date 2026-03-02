using DtoLayer.AuthDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResultDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new LoginResultDto
            {
                Success = false,
                Error = error
            };
          
        }
        return await response.Content.ReadFromJsonAsync<LoginResultDto>();
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("auth/logout", null);
    }

    public async Task<RegisterResultDto?> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", registerDto);
        return await response.Content.ReadFromJsonAsync<RegisterResultDto>();
    }

    public async Task<bool> SendEmailCodeAsync(string userId)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/send-email-code", userId);
        return response.IsSuccessStatusCode;
    }

    public async Task<LoginResultDto?> VerifyTwoFactorAsync(TwoFactorVerifyDto twoFactorVerifyDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/verify-2fa", twoFactorVerifyDto);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new LoginResultDto
            {
                Success = false,
                Error = error
            };

        }
        return await response.Content.ReadFromJsonAsync<LoginResultDto>();
    }
}
