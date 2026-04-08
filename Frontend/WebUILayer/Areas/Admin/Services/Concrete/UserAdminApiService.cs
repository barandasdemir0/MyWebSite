using DtoLayer.AuthDtos.Responses;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class UserAdminApiService : IUserAdminApiService
{
    private readonly HttpClient _httpClient;

    public UserAdminApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ApproveUserAsync(string userId, string role)
    {
        var response = await _httpClient.PostAsJsonAsync($"useradmin/approve-user/{userId}", role);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ApprovedUserDto>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetAsync("useradmin/all-users");
        if (!response.IsSuccessStatusCode)
        {
            return new List<ApprovedUserDto>();
        }
        return await response.Content.ReadFromJsonAsync<List<ApprovedUserDto>>() ?? new();
    }

    public async Task<List<PendingUserDto>> GetPendingUsersAsync()
    {
        var response = await _httpClient.GetAsync("useradmin/pending-users");
        if (!response.IsSuccessStatusCode)
        {
            return new List<PendingUserDto>();
        }
        return await response.Content.ReadFromJsonAsync<List<PendingUserDto>>() ?? new();
    }

    public async Task<bool> RejectUserAsync(string userId)
    {
        var response = await _httpClient.PostAsync($"useradmin/reject-user/{userId}", null);
        return response.IsSuccessStatusCode;
    }
}
