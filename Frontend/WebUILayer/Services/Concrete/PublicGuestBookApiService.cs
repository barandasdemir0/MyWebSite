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
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<GuestBookDto>();
        }
        return await response.Content.ReadFromJsonAsync<List<GuestBookDto>>() ?? new();
    }
}
