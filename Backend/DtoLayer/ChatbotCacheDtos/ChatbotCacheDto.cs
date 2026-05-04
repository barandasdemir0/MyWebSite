using SharedKernel.Shared;

namespace DtoLayer.ChatbotCacheDtos;

public class ChatbotCacheDto:IHasId
{
    public Guid Id { get; init; }
    public string UserQuestion { get; init; } = string.Empty;
    public string BotResponse { get; init; } = string.Empty;
}
