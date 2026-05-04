using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IProjectApiService:IGenericApiService<ProjectDto,CreateProjectDto,UpdateProjectDto>
    {
        Task<ProjectDto?> GetDetailById(Guid guid);
        Task<ProjectDto?> GetDetailBySlug(string slug);

        Task RestoreAsync(Guid guid);
        Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
        Task<List<ProjectDto>> GetLatestAsync(int count);
    }
}
