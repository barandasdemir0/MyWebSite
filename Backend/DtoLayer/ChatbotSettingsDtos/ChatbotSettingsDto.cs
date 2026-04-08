using SharedKernel.Shared;

namespace DtoLayer.ChatbotSettingsDtos;

public record ChatbotSettingsDto : IHasId
{
    public Guid Id { get; init; }
    public string AssistantName { get; init; } = string.Empty;
    public string WelcomeMessage { get; init; } = string.Empty;
    public string SystemPrompt { get; init; } = string.Empty;
    public string? ApiKey { get; init; }          // Şifrelenecek
    public string? ModelName { get; init; }       // "gpt-4o"
}