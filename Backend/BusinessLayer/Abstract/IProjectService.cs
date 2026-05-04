using CV.EntityLayer.Entities;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IProjectService:IGenericService<Project, ProjectDto, CreateProjectDto,UpdateProjectDto>
{

    Task<ProjectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<ProjectDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectDto>> GetAllUserAsync(PaginationQuery query, CancellationToken cancellationToken = default);

    Task<List<ProjectDto>> GetLatestAsync(int count, string? topic = null ,CancellationToken cancellationToken = default);
}
