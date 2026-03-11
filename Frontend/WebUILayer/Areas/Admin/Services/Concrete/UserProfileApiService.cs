using DtoLayer.AuthDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class UserProfileApiService : IUserProfileApiService
{
    private readonly HttpClient _httpClient;

    public UserProfileApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"userprofile/change-password", changePasswordDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"userprofile/profile");
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Hatası: {response.StatusCode} - {errorBody}");
        }
        return await response.Content.ReadFromJsonAsync<UserProfileDto>();
    }

    public async Task<bool> Toggle2FAAsync(string userId, Toggle2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"userprofile/toggle-2fa", dto);
        return response.IsSuccessStatusCode;
    }
}
