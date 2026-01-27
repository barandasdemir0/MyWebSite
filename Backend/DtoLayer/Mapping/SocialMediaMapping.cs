using CV.EntityLayer.Entities;
using DtoLayer.SkillDto;
using DtoLayer.SocialMediaDto;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class SocialMediaMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SocialMedia, SocialMediaDto.SocialMediaDto>();
            config.NewConfig<CreateSocialMediaDto, SocialMedia>().Ignore(x => x.Id);
            config.NewConfig<UpdateSocialMediaDto, SocialMedia>().Ignore(x => x.Id);
        }
    }
}
