using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicProjectApiService : PublicReadApiService<ProjectDto>, IPublicProjectApiService
{
    public PublicProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<PagedResult<ProjectDto>> GetAllPagedAsync(PaginationQuery paginationQuery)
    {
        var queryString = $"?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
        if (paginationQuery.TopicId.HasValue)
        {
            queryString += $"&TopicId={paginationQuery.TopicId}";
        }
        var response = await _httpClient.GetAsync($"{_endpoint}/user-all{queryString}");
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
            url += $"?Topic={topic}";
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
