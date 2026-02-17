namespace CV.EntityLayer.Entities;

public sealed class ChatbotSettings:BaseEntity
{
    public string AssistantName { get; set; } = string.Empty;
    public string WelcomeMessage { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public string? ApiKey { get; set; }          // Şifrelenecek
    public string? ModelName { get; set; }       // "gpt-4o"
}
