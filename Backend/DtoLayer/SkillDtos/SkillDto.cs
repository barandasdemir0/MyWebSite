using SharedKernel.Shared;

namespace DtoLayer.SkillDtos;

public class SkillDto : IHasId
{
    public Guid Id { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string IconifyIcon { get; set; } = string.Empty;

}
