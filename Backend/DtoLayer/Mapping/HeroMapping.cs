using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDto;
using DtoLayer.HeroDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class HeroMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Hero, HeroDto.HeroDto>();
            config.NewConfig<CreateHeroDto, Hero>().Ignore(x => x.Id);
            config.NewConfig<UpdateHeroDto, Hero>().Ignore(x => x.Id);
        }
    }
}
