using CV.EntityLayer.Entities;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IProjectService:IGenericService<Project, ProjectDto, CreateProjectDto,UpdateProjectDto>
{
    Task<ProjectDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<ProjectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<ProjectListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectListDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<ProjectListDto>> GetAllUserAsync(PaginationQuery query, CancellationToken cancellationToken = default);

    Task<List<ProjectDto>> GetLatestAsync(int count, CancellationToken cancellationToken = default);
}
