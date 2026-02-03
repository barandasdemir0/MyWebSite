using CV.EntityLayer.Entities;
using DtoLayer.EducationDtos;
using DtoLayer.ExperienceDtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class ExperienceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Experience, ExperienceDtos.ExperienceDto>();
        config.NewConfig<CreateExperienceDto, Experience>().Ignore(x => x.Id);
        config.NewConfig<UpdateExperienceDto, Experience>().Ignore(x => x.Id);
    }
}
