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
        await _httpClient.PostAsJsonAsync("account/forgot-password", forgotPasswordDto); // API'ye forgot password isteği gönderir. Bu istek, kullanıcının email adresine OTP gönderilmesini sağlar.
    }

    public async Task<string?> VerifyResetOtpAsync(VerifyResetOtpDto verifyResetOtpDto)
    {
        // API'ye verify reset OTP isteği gönderir. Bu istek, kullanıcının girdiği OTP'nin doğruluğunu kontrol eder. Eğer OTP doğruysa, API bir reset token döner. Bu token, kullanıcının yeni şifre belirlemesi için gereklidir.
        var result = await _httpClient.PostAsJsonAsync("account/verify-reset-otp", verifyResetOtpDto);
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        var body = await result.Content.ReadFromJsonAsync<JsonElement>();// API'den dönen yanıtın gövdesini JSON formatında okur ve reset token'ı çıkarır. neden jsonelement kullandık : Çünkü API'den dönen yanıtın yapısı önceden bilinmediği için, JSON verisini dinamik olarak işlemek için JsonElement kullanılır.
        return body.GetProperty("resetToken").GetString(); // JSON verisinden "resetToken" adlı özelliği alır ve string olarak döner. Bu token, kullanıcının yeni şifre belirlemesi için gereklidir.

    }

    public async Task<bool> SetNewPasswordAsync(SetNewPasswordDto setNewPasswordDto)
    {
        var result = await _httpClient.PostAsJsonAsync("account/set-new-password", setNewPasswordDto);
        return result.IsSuccessStatusCode; // API'ye set new password isteği gönderir. Bu istek, kullanıcının yeni şifresini belirlemesini sağlar. Eğer istek başarılıysa, true döner; aksi takdirde false döner.
    }
}
