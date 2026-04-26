using DtoLayer.ProjectDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicProjectApiService : PublicReadApiService<ProjectListDto>, IPublicProjectApiService
{
    public PublicProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<List<ProjectListDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<ProjectListDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<ProjectListDto>>();
        if (result == null)
        {
            return new List<ProjectListDto>();
        }
        return result;
    }
}
