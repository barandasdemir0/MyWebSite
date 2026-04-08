using SharedKernel.Shared;

namespace DtoLayer.HeroDtos;

public record HeroDto : IHasId
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string ProfessionalTitle { get; init; } = string.Empty;
    public string ScrollingText { get; init; } = string.Empty;
    public string HeroAbout { get; init; } = string.Empty;
}
