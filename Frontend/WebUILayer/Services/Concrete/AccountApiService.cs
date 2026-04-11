using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using System.Text.Json;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class AccountApiService : IAccountApiService
{
    private readonly HttpClient _httpClient;

    public AccountApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        await _httpClient.PostAsJsonAsync("auth/forgot-password", forgotPasswordDto);
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
