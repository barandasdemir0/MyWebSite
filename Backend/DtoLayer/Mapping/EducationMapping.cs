using CV.EntityLayer.Entities;
using DtoLayer.ContactDtos;
using DtoLayer.EducationDtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class EducationMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Education, EducationDtos.EducationDto>();
            config.NewConfig<CreateEducationDto, Education>().Ignore(x => x.Id);
            config.NewConfig<UpdateEducationDto, Education>().Ignore(x => x.Id);
        }
    }
}
