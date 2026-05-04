using DtoLayer.ChatbotCacheDtos;
using EntityLayer.Entities;
using Mapster;

namespace BusinessLayer.Mapping;

public class ChatbotCacheMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ChatbotCache, ChatbotCacheDto>();
        config.NewConfig<CreateChatbotCacheDto, ChatbotCache>();
        config.NewConfig<UpdateChatbotCacheDto, ChatbotCache>();
    }
}
