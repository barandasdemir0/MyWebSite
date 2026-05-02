using CV.EntityLayer.Entities;
using DtoLayer.EducationDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class EducationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Education, EducationDtos.EducationDto>();
        config.NewConfig<CreateEducationDto, Education>().Ignore(x => x.Id);
        config.NewConfig<UpdateEducationDto, Education>().Ignore(x => x.Id);
    }
}
