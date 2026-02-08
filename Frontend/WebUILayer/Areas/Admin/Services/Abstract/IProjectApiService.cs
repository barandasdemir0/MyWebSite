using DtoLayer.ProjectDtos;
using DtoLayer.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IProjectApiService:IGenericApiService<ProjectDto,CreateProjectDto,UpdateProjectDto>
    {
        Task<ProjectListDto?> GetDetailtById(Guid guid);
        Task<ProjectListDto?> GetDetailBySlug(string slug);

        Task RestoreAsync(Guid guid);
        Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
    }
}
