using CV.EntityLayer.Entities;
using DtoLayer.SocialMediaDto;
using DtoLayer.TopicDto;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class TopicMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Topic, TopicDto.TopicDto>();
            config.NewConfig<CreateTopicDto, Topic>().Ignore(x => x.Id);
            config.NewConfig<UpdateTopicDto, Topic>().Ignore(x => x.Id);
        }
    }
}
