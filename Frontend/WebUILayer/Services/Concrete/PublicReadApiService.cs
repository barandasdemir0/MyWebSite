using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicReadApiService<T> : IPublicReadApiService<T>
{
    protected readonly HttpClient _httpClient;
    protected readonly string _endpoint;

    public PublicReadApiService(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_endpoint);
        if (!response.IsSuccessStatusCode)
        {
            return new List<T>();
        }

        var data = await response.Content.ReadFromJsonAsync<List<T>>();
        if (data==null)
        {
            return new List<T>();
        }
        return data;
    }
}
