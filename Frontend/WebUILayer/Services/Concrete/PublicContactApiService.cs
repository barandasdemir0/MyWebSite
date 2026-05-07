using DtoLayer.ContactDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicContactApiService : PublicReadApiService<ContactDto>, IPublicContactApiService
{
    public PublicContactApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "contacts")
    {
    }

    public async Task<ContactDto> GetPublicContactSettingsAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/single");
        if (!response.IsSuccessStatusCode) return new ContactDto();
        return await response.Content.ReadFromJsonAsync<ContactDto>() ?? new ContactDto();
    }
}
