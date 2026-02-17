using CV.EntityLayer.Entities;
using DtoLayer.JobSkillCategoryDtos;

namespace BusinessLayer.Abstract;

public interface IJobSkillCategoryService : IGenericService<JobSkillCategory, JobSkillCategoryDto, CreateJobSkillCategoryDto, UpdateJobSkillCategoryDto>
{
    Task<JobSkillCategoryDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<JobSkillCategoryDto>> GetAdminAllAsync( CancellationToken cancellationToken = default);
}
