using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Shared;
using WebUILayer.Extension;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicProjectApiService : PublicReadApiService<ProjectDto>, IPublicProjectApiService
{
    public PublicProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<PagedResult<ProjectDto>> GetAllPagedAsync(PaginationQuery paginationQuery)
    {
    
        var url = paginationQuery.ToQueryString($"{_endpoint}/user-all");

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return new PagedResult<ProjectDto>
            {
                Items = new List<ProjectDto>()
            };
        }
        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProjectDto>>();
        if (result==null)
        {
            return new PagedResult<ProjectDto>
            {
                Items = new List<ProjectDto>()
            };
        }
        return result;
    }

    public async Task<ProjectDto?> GetBySlugAsync(string slug)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/slug/{slug}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
        return result;
    }

    public async Task<List<ProjectDto>> GetLatestAsync(int count, string? topic = null)
    {
        var url = $"{_endpoint}/latest/{count}";
        if (!string.IsNullOrEmpty(topic))
        {
            url = QueryHelpers.AddQueryString(url, "Topic", topic);
        }

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return new List<ProjectDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
        if (result == null)
        {
            return new List<ProjectDto>();
        }
        return result;
    }
}
