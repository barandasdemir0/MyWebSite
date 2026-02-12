using DtoLayer.JobSkillCategoryDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IJobSkillCategoryService : IGenericApiService<JobSkillCategoryDto, CreateJobSkillCategoryDto, UpdateJobSkillCategoryDto>
{
    Task<List<JobSkillCategoryDto>> GetAdminAllAsync();
    Task RestoreAsync(Guid guid);
}
