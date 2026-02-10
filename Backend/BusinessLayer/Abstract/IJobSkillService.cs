using DtoLayer.JobSkillsDtos;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface IJobSkillService:IGenericService<JobSkill,JobSkillDto,CreateJobSkillDto,UpdateJobSkillDto>
{
    Task<JobSkillDto?> RestoreAsync(Guid guid);
    Task<List<JobSkillDto>> GetAdminAllAsync();
}
