using SharedKernel.Shared;

namespace DtoLayer.AboutDtos;

public record AboutDto:IHasId
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Greeting { get; init; } = string.Empty;
    public string Bio { get; init; } = string.Empty;
    public string? ProfileImage { get; init; }
    public int ProjectCount { get; init; }
    public int ExperienceYear { get; init; }
    public int ProjectDrink { get; init; }
}
