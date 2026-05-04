using CV.EntityLayer.Entities;
using DtoLayer.JobSkillsDtos;
using Mapster;

namespace DtoLayer.Mapping;

public class JobSkillMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JobSkill, JobSkillsDtos.JobSkillDto>()
            .Map(x => x.CategoryName, y => y.JobSkillCategory.CategoryName);
        config.NewConfig<CreateJobSkillDto, JobSkill>().Ignore(x => x.Id);
        config.NewConfig<UpdateJobSkillDto, JobSkill>().Ignore(x => x.Id);
    }
}
