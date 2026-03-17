using DtoLayer.AuthDtos;
using System.Net.Http.Headers;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient; 
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        await _httpClient.PostAsJsonAsync("auth/forgot-password", forgotPasswordDto);
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

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/reset-password", resetPasswordDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SetNewPasswordAsync(SetNewPasswordDto setNewPasswordDto)
    {
        var result = await _httpClient.PostAsJsonAsync("auth/set-new-password", setNewPasswordDto);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> VerifyResetOtpAsync(VerifyResetOtpDto verifyResetOtpDto)
    {

        var result = await _httpClient.PostAsJsonAsync("auth/verify-reset-otp", verifyResetOtpDto);
        return result.IsSuccessStatusCode;
      
    }
}
