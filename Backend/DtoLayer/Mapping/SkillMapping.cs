using CV.EntityLayer.Entities;
using DtoLayer.HeroDto;
using DtoLayer.SkillDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class SkillMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Skill, SkillDto.SkillDto>();
            config.NewConfig<CreateSkillDto, Skill>().Ignore(x => x.Id);
            config.NewConfig<UpdateSkillDto, Skill>().Ignore(x => x.Id);
        }
    }
}
