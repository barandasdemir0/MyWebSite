using CV.EntityLayer.Entities;
using DtoLayer.HeroDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class HeroMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Hero, HeroDtos.HeroDto>();
        config.NewConfig<CreateHeroDto, Hero>().Ignore(x => x.Id);
        config.NewConfig<UpdateHeroDto, Hero>().Ignore(x => x.Id);
    }
}
