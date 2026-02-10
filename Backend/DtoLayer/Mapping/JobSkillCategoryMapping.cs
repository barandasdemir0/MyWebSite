using DtoLayer.JobSkillCategoryDtos;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class JobSkillCategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JobSkillCategory, JobSkillCategoryDtos.JobSkillCategoryDto>();
        config.NewConfig<CreateJobSkillCategoryDto, JobSkillCategory>().Ignore(dest => dest.Id);
        config.NewConfig<UpdateJobSkillCategoryDto, JobSkillCategory>().Ignore(dest => dest.Id);
    }
}
