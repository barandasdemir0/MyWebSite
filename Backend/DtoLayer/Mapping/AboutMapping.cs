using CV.EntityLayer.Entities;
using DtoLayer.AboutDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class AboutMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<About, AboutDto.AboutDto>();
        config.NewConfig<CreateAboutDto, About>().Ignore(dest=>dest.Id);
        config.NewConfig<UpdateAboutDto, About>().Ignore(dest=>dest.Id);
    }
}
