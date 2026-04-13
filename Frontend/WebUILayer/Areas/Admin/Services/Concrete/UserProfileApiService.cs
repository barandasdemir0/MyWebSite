using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class UserProfileApiService : IUserProfileApiService
{
    private readonly HttpClient _httpClient;

    public UserProfileApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var response = await _httpClient.PostAsJsonAsync("userprofile/change-password", changePasswordDto); // post nedir :  post metodu, sunucuya veri göndermek için kullanılır. postasjsonasync ile veriyi json formatında gönderiyoruz. "userprofile/change-password" ise api endpoint'idir.
        return response.IsSuccessStatusCode;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync()
    {
        var response = await _httpClient.GetAsync("userprofile/profile"); // get nedir : get metodu, sunucudan veri almak için kullanılır. getasync ile belirtilen endpoint'e bir GET isteği gönderiyoruz. "userprofile/profile" ise api endpoint'idir.
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(); // eğer istek başarısız olduysa, hata mesajını okuyup bir istisna fırlatıyoruz. readasstringasync ile response body'sini string olarak okuyoruz.
            throw new Exception($"API Hatası: {response.StatusCode} - {errorBody}"); // response.statuscode ile HTTP durum kodunu, errorbody ile ise API'den dönen hata mesajını alıyoruz. bu sayede, hataların daha kolay anlaşılmasını ve yönetilmesini sağlıyoruz.
        }
        return await response.Content.ReadFromJsonAsync<UserProfileDto>(); // eğer istek başarılı olduysa, response body'sini UserProfileDto nesnesine dönüştürüp döndürüyoruz. readfromjsonasync ile response body'sini JSON formatından UserProfileDto nesnesine dönüştürüyoruz.
    }

    public async Task<bool> Toggle2FAAsync(Toggle2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("userprofile/toggle-2fa", dto); //burada kısaca kullanıcıların iki faktörlü kimlik doğrulamasını etkinleştirmek veya devre dışı bırakmak için bir API çağrısı yapıyoruz. postasjsonasync ile dto nesnesini JSON formatında
        return response.IsSuccessStatusCode;
    }
}
