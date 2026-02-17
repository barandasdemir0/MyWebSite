namespace DtoLayer.ChatbotSettingsDtos;

public class ChatbotSettingsDto
{
    public Guid Id { get; set; }
    public string AssistantName { get; set; } = string.Empty;
    public string WelcomeMessage { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public string? ApiKey { get; set; }          // Şifrelenecek
    public string? ModelName { get; set; }       // "gpt-4o"
}
