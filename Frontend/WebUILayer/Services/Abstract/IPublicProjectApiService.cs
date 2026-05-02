using DtoLayer.ProjectDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicProjectApiService:IPublicReadApiService<ProjectDto>
{
    Task<List<ProjectDto>> GetLatestAsync(int count , string? topic = null);
}
