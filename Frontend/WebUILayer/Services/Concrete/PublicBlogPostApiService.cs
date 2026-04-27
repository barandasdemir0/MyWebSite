using DtoLayer.BlogPostDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicBlogPostApiService : PublicReadApiService<BlogPostDto>, IPublicBlogPostApiService
{
    public PublicBlogPostApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "blogposts")
    {
    }

    public async Task<List<BlogPostDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<BlogPostDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<BlogPostDto>>();
        if (result == null)
        {
            return new List<BlogPostDto>();
        }
        return result;
    }
}
