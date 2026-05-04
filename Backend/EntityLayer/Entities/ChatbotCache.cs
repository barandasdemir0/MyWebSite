using CV.EntityLayer.Entities;

namespace EntityLayer.Entities;

public class ChatbotCache:BaseEntity
{
    public string UserQuestion { get; set; } = string.Empty;
    public string BotResponse { get; set; } = string.Empty;
}
