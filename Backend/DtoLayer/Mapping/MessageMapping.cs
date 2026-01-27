using CV.EntityLayer.Entities;
using DtoLayer.HeroDto;
using DtoLayer.MessageDto;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class MessageMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Message, MessageDto.MessageDto>();
            config.NewConfig<Message, MessageDto.MessageListDto>();
            config.NewConfig<CreateMessageDto, Message>().Ignore(x => x.Id);
            config.NewConfig<UpdateMessageDto, Message>().Ignore(x => x.Id);
        }
    }
}
