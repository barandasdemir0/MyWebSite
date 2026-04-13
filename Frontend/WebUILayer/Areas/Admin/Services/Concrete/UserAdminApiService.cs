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

    // Kullanıcı onaylama işlemi için API'ye POST isteği gönderir
    public async Task<bool> ApproveUserAsync(string userId, string role)
    {
        var response = await _httpClient.PostAsJsonAsync($"useradmin/approve-user/{userId}", role); // API endpoint'ine kullanıcı ID'si ve rol bilgisi gönderilir
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ApprovedUserDto>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetAsync("useradmin/all-users"); // API endpoint'ine tüm kullanıcıları almak için GET isteği gönderilir
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
        return await response.Content.ReadFromJsonAsync<List<PendingUserDto>>() ?? new(); // response içeriği JSON formatında olduğu için önce okunur, sonra PendingUserDto listesine dönüştürülür. Eğer dönüşüm başarısız olursa boş liste döndürülür.
    }

    public async Task<bool> RejectUserAsync(string userId)
    {
        var response = await _httpClient.PostAsync($"useradmin/reject-user/{userId}", null); // API endpoint'ine kullanıcı ID'si gönderilir, body boş bırakılır çünkü reddetme işlemi için ekstra bilgi gerekmez
        return response.IsSuccessStatusCode;
    }
}
