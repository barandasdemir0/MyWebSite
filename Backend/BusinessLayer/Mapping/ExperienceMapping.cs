using CV.EntityLayer.Entities;
using DtoLayer.ExperienceDtos;
using Mapster;

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
