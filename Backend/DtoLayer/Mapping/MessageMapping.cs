using CV.EntityLayer.Entities;
using DtoLayer.HeroDtos;
using DtoLayer.MessageDtos;
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
            config.NewConfig<Message, MessageDtos.MessageDto>();
            config.NewConfig<Message, MessageDtos.MessageListDto>();
            config.NewConfig<CreateMessageDto, Message>().Ignore(x => x.Id);
            config.NewConfig<UpdateMessageDto, Message>().Ignore(x => x.Id);
        }
    }
}
