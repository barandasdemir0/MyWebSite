using CV.EntityLayer.Entities;
using DtoLayer.ChatbotSettingsDtos;
using Mapster;

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
