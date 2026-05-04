using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace WebUILayer.Services.Abstract;

public interface IPublicProjectApiService:IPublicReadApiService<ProjectDto>
{
    Task<List<ProjectDto>> GetLatestAsync(int count , string? topic = null);
    Task<ProjectDto?> GetBySlugAsync(string slug);
    Task<PagedResult<ProjectDto>> GetAllPagedAsync(PaginationQuery paginationQuery);
}
