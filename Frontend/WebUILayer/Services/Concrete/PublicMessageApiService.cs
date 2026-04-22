using DtoLayer.MessageDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicMessageApiService : IPublicMessageApiService
{
    private readonly HttpClient _httpClient;

    public PublicMessageApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SendContactMessageAsync(CreateMessageDto createMessageDto)
    {
        var response = await _httpClient.PostAsJsonAsync("messages", createMessageDto);
        return response.IsSuccessStatusCode;
    }
}
