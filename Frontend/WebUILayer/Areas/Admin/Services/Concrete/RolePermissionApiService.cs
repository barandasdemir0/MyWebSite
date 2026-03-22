using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class RolePermissionApiService : IRolePermissionApiService
{
    private readonly HttpClient _httpClient;

    public RolePermissionApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName)
    {
        var response = await _httpClient.GetAsync($"rolepermission/{roleName}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<string>();
        }
        return await response.Content.ReadFromJsonAsync<List<string>>() ?? new();
    }

    public async Task<bool> SaveRolePermissions(string roleName, List<string> permissions)
    {
        var response = await _httpClient.PostAsJsonAsync($"rolepermission/{roleName}", permissions);
        return response.IsSuccessStatusCode;
    }
}
