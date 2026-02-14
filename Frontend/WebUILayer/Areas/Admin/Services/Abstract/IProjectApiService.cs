using DtoLayer.BlogpostDtos;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IProjectApiService:IGenericApiService<ProjectDto,CreateProjectDto,UpdateProjectDto>
    {
        Task<ProjectListDto?> GetDetailtById(Guid guid);
        Task<ProjectListDto?> GetDetailBySlug(string slug);

        Task RestoreAsync(Guid guid);
        Task<PagedResult<ProjectDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
        Task<List<ProjectDto>> GetLatestAsync(int count);
    }
}
