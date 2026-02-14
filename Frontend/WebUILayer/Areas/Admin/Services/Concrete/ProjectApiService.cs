using DtoLayer.ProjectDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class ProjectApiService : GenericApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>, IProjectApiService
{
    public ProjectApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "projects")
    {
    }

    public async Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery paginationQuery)
    {
        var url = $"{_endpoint}/admin-all?PageNumber={paginationQuery.PageNumber}&PageSize={paginationQuery.PageSize}";
        if (paginationQuery.TopicId.HasValue)
        {
            url += $"&TopicId={paginationQuery.TopicId}";
        }
        var result = await _httpClient.GetFromJsonAsync<PagedResult<ProjectDto>>(url);
        if (result== null)
        {
            return new PagedResult<ProjectDto>();
        }
        return result;
    }

    public async Task<ProjectListDto?> GetDetailBySlug(string slug)
    {
        return await _httpClient.GetFromJsonAsync<ProjectListDto>($"{_endpoint}/{slug}");
    }

    public async Task<ProjectListDto?> GetDetailtById(Guid guid)
    {
        return await _httpClient.GetFromJsonAsync<ProjectListDto>($"{_endpoint}/{guid}");
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
