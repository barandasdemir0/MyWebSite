using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class RolePermissionApiService : IRolePermissionApiService
{
    private readonly HttpClient _httpClient;

    public RolePermissionApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Belirli bir role ait izinleri API'den alır
    public async Task<List<string>> GetRolePermissionsAsync(string roleName)
    {
        var response = await _httpClient.GetAsync($"rolepermission/{roleName}"); //getasync ile rolepermission/{roleName} endpoint'ine istek gönderir
        if (!response.IsSuccessStatusCode)// Başarısızsa boş liste döner
        {
            return new List<string>();  // API'den izinler alınamazsa boş liste döndür
        }
        return await response.Content.ReadFromJsonAsync<List<string>>() ?? new(); // Başarılıysa JSON'ı List<string> olarak dönüştürür ve döner ?? new() null durumunda boş liste döndürür yani doluysa liste döner, boşsa boş liste döner
    }

    // Belirli bir role ait izinleri API'ye kaydeder
    public async Task<bool> SaveRolePermissions(string roleName, List<string> permissions)
    {
        var response = await _httpClient.PostAsJsonAsync($"rolepermission/{roleName}", permissions);// POST isteği ile rolepermission/{roleName} endpoint'ine izin listesini JSON olarak gönderir postasjsonasync ile postasync farkı şöyledir: postasync sadece endpoint'e istek gönderirken, postasjsonasync ise DTO'yu JSON formatında body'ye ekleyerek gönderir. Bu durumda izin listesini JSON olarak göndermek istediğimiz için postasjsonasync kullanılır.
        return response.IsSuccessStatusCode; 
    }
}
