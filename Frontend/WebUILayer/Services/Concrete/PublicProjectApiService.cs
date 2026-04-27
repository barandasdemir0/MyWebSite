using DtoLayer.ProjectDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicProjectApiService : PublicReadApiService<ProjectDto>, IPublicProjectApiService
{
    public PublicProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<List<ProjectDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
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
