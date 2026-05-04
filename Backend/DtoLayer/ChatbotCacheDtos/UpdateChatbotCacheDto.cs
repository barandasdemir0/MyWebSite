using SharedKernel.Shared;

namespace DtoLayer.ChatbotCacheDtos;

public class UpdateChatbotCacheDto:IHasId
{
    public Guid Id { get; set; }
    public string UserQuestion { get; set; } = string.Empty;
    public string BotResponse { get; set; } = string.Empty;
}
