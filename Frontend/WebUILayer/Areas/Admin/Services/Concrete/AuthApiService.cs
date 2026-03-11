using DtoLayer.AuthDtos;
using System.Net.Http.Headers;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"auth/change-password/{userId}", changePasswordDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ConfirmAuthenticatorAsync(TwoFactorVerifyDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Confirm-authenticator", dto);
        return response.IsSuccessStatusCode;
    }


  

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
    {

        var response = await _httpClient.GetAsync($"auth/profile/{userId}");
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Hatası: {response.StatusCode} - {errorBody}");
        }
        return await response.Content.ReadFromJsonAsync<UserProfileDto>();
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
        if (!response.IsSuccessStatusCode)
        {
            var errors = await response.Content.ReadFromJsonAsync<List<string>>();
            return new RegisterResultDto
            {
                Success = false,
                Errors = errors?? new List<string>
                {
                    "Kayıt Başarısız. Girdiğiniz bilgileri kontrol edip tekrar deneyin."
                }
            };
        }
        return await response.Content.ReadFromJsonAsync<RegisterResultDto>();
    }

    public async Task<bool> SendEmailCodeAsync(string userId)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/send-email-code", userId);
        return response.IsSuccessStatusCode;
    }

    public async Task<Setup2FAResultDto?> SetupAuthenticatorAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"auth/setup-authenticator/{userId}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<Setup2FAResultDto>();
    }

    public async Task<bool> Toggle2FAAsync(string userId, Toggle2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"auth/toggle-2fa/{userId}", dto);
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
