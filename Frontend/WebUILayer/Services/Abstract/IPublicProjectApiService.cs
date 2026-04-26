using DtoLayer.ProjectDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicProjectApiService:IPublicReadApiService<ProjectListDto>
{
    Task<List<ProjectListDto>> GetLatestAsync(int count);
}
