using DtoLayer.GuestBookDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicGuestBookApiService : PublicReadApiService<GuestBookDto>, IPublicGuestBookApiService
{
    public PublicGuestBookApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "guestbooks")
    {
    }

    public async Task<List<GuestBookDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"api/guestbooks/latest/{count}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<GuestBookDto>>() ?? new();
    }
}
