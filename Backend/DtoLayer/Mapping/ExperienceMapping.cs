using CV.EntityLayer.Entities;
using DtoLayer.EducationDto;
using DtoLayer.ExperienceDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class ExperienceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Experience, ExperienceDto.ExperienceDto>();
        config.NewConfig<CreateExperienceDto, Experience>().Ignore(x => x.Id);
        config.NewConfig<UpdateExperienceDto, Experience>().Ignore(x => x.Id);
    }
}
