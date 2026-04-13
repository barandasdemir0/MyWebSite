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

  
    public async Task<LoginResultDto?> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto); // API'ye login bilgilerini gönderir ve sonucu alır.
        if (!response.IsSuccessStatusCode) 
        {
            var error = await response.Content.ReadAsStringAsync();
            return new LoginResultDto
            {
                Success = false,
                Error = error
            };
          
        }
        return await response.Content.ReadFromJsonAsync<LoginResultDto>();// Başarılıysa, API'den gelen sonucu LoginResultDto olarak döndürür.
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("auth/logout", null);// API'ye logout isteği gönderir. Genellikle token'ı geçersiz kılmak için kullanılır.
    }

    public async Task<RegisterResultDto?> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", registerDto);// API'ye kayıt bilgilerini gönderir ve sonucu alır.
        if (!response.IsSuccessStatusCode)
        {
            var errors = await response.Content.ReadFromJsonAsync<List<string>>(); 
            return new RegisterResultDto
            {
                Success = false,
                Errors = errors ?? new List<string> // Eğer API'den hata mesajları gelmezse, varsayılan olarak tek bir genel hata mesajı içeren bir liste döndürür.
                {
                    "Kayıt Başarısız. Girdiğiniz bilgileri kontrol edip tekrar deneyin."
                }
            };
        }
        return await response.Content.ReadFromJsonAsync<RegisterResultDto>();// Başarılıysa, API'den gelen sonucu RegisterResultDto olarak döndürür.
    }




}
