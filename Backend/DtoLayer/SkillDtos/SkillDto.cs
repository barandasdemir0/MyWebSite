using SharedKernel.Shared;

namespace DtoLayer.SkillDtos;

public record SkillDto : IHasId
{
    public Guid Id { get; init; }
    public string SkillName { get; init; } = string.Empty;
    public string IconifyIcon { get; init; } = string.Empty;

}
