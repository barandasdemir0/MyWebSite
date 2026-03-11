using DtoLayer.AuthDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class TwoFactorApiService : ITwoFactorApiService
{
    private readonly HttpClient _httpClient;

    public TwoFactorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ConfirmAuthenticatorAsync(TwoFactorVerifyDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Confirm-authenticator", dto);
        return response.IsSuccessStatusCode;
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
