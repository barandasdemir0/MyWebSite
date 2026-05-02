using DtoLayer.ProjectDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicProjectApiService : PublicReadApiService<ProjectDto>, IPublicProjectApiService
{
    public PublicProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<List<ProjectDto>> GetLatestAsync(int count, string? topic = null)
    {
        var url = $"{_endpoint}/latest/{count}";
        if (!string.IsNullOrEmpty(topic))
        {
            url += $"?topic={topic}";
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
