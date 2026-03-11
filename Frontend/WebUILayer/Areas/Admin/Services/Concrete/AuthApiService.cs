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


}
