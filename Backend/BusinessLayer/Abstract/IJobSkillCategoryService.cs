using DtoLayer.JobSkillCategoryDtos;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface IJobSkillCategoryService : IGenericService<JobSkillCategory, JobSkillCategoryDto, CreateJobSkillCategoryDto, UpdateJobSkillCategoryDto>
{
    Task<JobSkillCategoryDto?> RestoreAsync(Guid guid);
    Task<List<JobSkillCategoryDto>> GetAdminAllAsync();
}
