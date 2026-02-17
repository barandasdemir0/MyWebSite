using CV.EntityLayer.Entities;
using DtoLayer.SkillDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class SkillMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Skill, SkillDtos.SkillDto>();
        config.NewConfig<CreateSkillDto, Skill>().Ignore(x => x.Id);
        config.NewConfig<UpdateSkillDto, Skill>().Ignore(x => x.Id);
    }
}
