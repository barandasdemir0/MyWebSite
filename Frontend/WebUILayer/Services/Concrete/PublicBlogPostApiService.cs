using DtoLayer.BlogPostDtos;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Shared;
using WebUILayer.Extension;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicBlogPostApiService : PublicReadApiService<BlogPostDto>, IPublicBlogPostApiService
{
    public PublicBlogPostApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "blogposts")
    {
    }

    public async Task<PagedResult<BlogPostDto>> GetAllPagedAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/user-all");

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new PagedResult<BlogPostDto>
            {
                Items = new List<BlogPostDto>()
            };
        }

        var result = await response.Content.ReadFromJsonAsync<PagedResult<BlogPostDto>>();
        if (result==null)
        {
            return new PagedResult<BlogPostDto>()
            {
                Items = new List<BlogPostDto>()
            };
        }
        return result;
    }

    public async Task<BlogPostDto?> GetBySlugAsync(string slug)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/slug/{slug}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var result = await response.Content.ReadFromJsonAsync<BlogPostDto>();
        return result;
        
    }

    public async Task<List<BlogPostDto>> GetLatestAsync(int count, string? topic = null)
    {
        var url = $"{_endpoint}/latest/{count}";
        if (!string.IsNullOrEmpty(topic))
        {
            url = QueryHelpers.AddQueryString(url, "Topic", topic);
        }
        var response = await _httpClient.GetAsync(url);
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
