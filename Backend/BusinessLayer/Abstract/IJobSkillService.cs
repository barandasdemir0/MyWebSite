using CV.EntityLayer.Entities;
using DtoLayer.JobSkillsDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface IJobSkillService:IGenericService<JobSkill,JobSkillDto,CreateJobSkillDto,UpdateJobSkillDto>
{
    Task<JobSkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<JobSkillDto>> GetAdminAllAsync( CancellationToken cancellationToken = default);
}
