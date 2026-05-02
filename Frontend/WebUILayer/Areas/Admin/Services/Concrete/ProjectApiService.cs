using DtoLayer.ProjectDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Extension;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class ProjectApiService : GenericApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>, IProjectApiService
{
    public ProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery paginationQuery)
    {
        var url = paginationQuery.ToQueryString($"{_endpoint}/admin-all");
        var result = await _httpClient.GetFromJsonAsync<PagedResult<ProjectDto>>(url);
        if (result== null)
        {
            return new PagedResult<ProjectDto>();
        }
        return result;
    }

    public async Task<ProjectDto?> GetDetailBySlug(string slug)
    {
        return await _httpClient.GetFromJsonAsync<ProjectDto>($"{_endpoint}/{slug}");
    }

    public async Task<ProjectDto?> GetDetailById(Guid guid)
    {
        return await _httpClient.GetFromJsonAsync<ProjectDto>($"{_endpoint}/{guid}");
    }

    public async Task<List<ProjectDto>> GetLatestAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/latest/{count}");
        if (!response.IsSuccessStatusCode)
        {
            return new List<ProjectDto>();
        }
        var result = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();
        if (result==null)
        {
            return new List<ProjectDto>();
        }
        return result;
    }

    public async Task RestoreAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
