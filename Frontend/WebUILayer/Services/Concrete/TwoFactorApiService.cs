using DtoLayer.AuthDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class TwoFactorApiService : ITwoFactorApiService
{
    private readonly HttpClient _httpClient;

    public TwoFactorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ConfirmAuthenticatorAsync(TwoFactorVerifyDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("twofactor/confirm-authenticator", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendEmailCodeAsync(string userId)
    {
        var response = await _httpClient.PostAsJsonAsync("twofactor/send-email-code", new
        {
            UserId = userId
        });
        return response.IsSuccessStatusCode;
    }

    public async Task<Setup2FAResultDto?> SetupAuthenticatorAsync()
    {
        var response = await _httpClient.GetAsync($"twofactor/setup-authenticator");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<Setup2FAResultDto>();
    }

    public async Task<LoginResultDto?> VerifyTwoFactorAsync(TwoFactorVerifyDto twoFactorVerifyDto)
    {
        var response = await _httpClient.PostAsJsonAsync("twofactor/verify-2fa", twoFactorVerifyDto);
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
