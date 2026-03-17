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

    public async Task<bool> ApproveUserAsync(string userId,string role)
    {
        var response = await _httpClient.PostAsJsonAsync($"userprofile/approve-user/{userId}",role);
        return response.IsSuccessStatusCode;
    }

  

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var response = await _httpClient.PostAsJsonAsync("userprofile/change-password", changePasswordDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<PendingUserDto>> GetPendingUsersAsync()
    {
        var response = await _httpClient.GetAsync("userprofile/pending-users");
        if (!response.IsSuccessStatusCode)
        {
            return new List<PendingUserDto>();
        }
        return await response.Content.ReadFromJsonAsync<List<PendingUserDto>>() ?? new();
    }

    public async Task<UserProfileDto?> GetUserProfileAsync()
    {
        var response = await _httpClient.GetAsync("userprofile/profile");
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Hatası: {response.StatusCode} - {errorBody}");
        }
        return await response.Content.ReadFromJsonAsync<UserProfileDto>();
    }

    public async Task<bool> RejectUserAsync(string userId)
    {
        var response = await _httpClient.PostAsync($"userprofile/reject-user/{userId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Toggle2FAAsync(Toggle2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("userprofile/toggle-2fa", dto);
        return response.IsSuccessStatusCode;
    }
}
