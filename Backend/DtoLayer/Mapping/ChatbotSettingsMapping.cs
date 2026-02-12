using DtoLayer.ChatbotSettingsDtos;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public class ChatbotSettingsMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ChatbotSettings, ChatbotSettingsDtos.ChatbotSettingsDto>();
        config.NewConfig<CreateChatbotSettingsDto , ChatbotSettings>().Ignore(x=>x.Id);
        config.NewConfig<UpdateChatbotSettingsDto , ChatbotSettings>().Ignore(x=>x.Id);
    }
}
