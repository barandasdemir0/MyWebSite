using DtoLayer.JobSkillsDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IJobSkillApiService:IGenericApiService<JobSkillDto,CreateJobSkillDto,UpdateJobSkillDto>
{
    Task<List<JobSkillDto>> GetAllAdminAsync();
    Task RestoreAsync(Guid guid);
}
