using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using System.Net.Http.Headers;
using System.Text.Json;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

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
   
    public async Task<string?> VerifyResetOtpAsync(VerifyResetOtpDto verifyResetOtpDto)
    {

        var result = await _httpClient.PostAsJsonAsync("auth/verify-reset-otp", verifyResetOtpDto);
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        var body = await result.Content.ReadFromJsonAsync<JsonElement>();
        return body.GetProperty("resetToken").GetString();
      
    }

    public async Task<bool> SetNewPasswordAsync(SetNewPasswordDto setNewPasswordDto)
    {
        var result = await _httpClient.PostAsJsonAsync("auth/set-new-password", setNewPasswordDto);
        return result.IsSuccessStatusCode;
    }

}
