using CV.EntityLayer.Entities;
using DtoLayer.SkillDtos;
using DtoLayer.SocialMediaDtos;
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
            config.NewConfig<SocialMedia, SocialMediaDtos.SocialMediaDto >();
            config.NewConfig<CreateSocialMediaDto, SocialMedia>().Ignore(x => x.Id);
            config.NewConfig<UpdateSocialMediaDto, SocialMedia>().Ignore(x => x.Id);
        }
    }
}
