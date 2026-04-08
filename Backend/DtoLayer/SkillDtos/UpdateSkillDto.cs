using SharedKernel.Shared;

namespace DtoLayer.SkillDtos;

public class UpdateSkillDto : IHasId
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string SkillName { get; set; } = string.Empty;
    public string IconifyIcon { get; set; } = string.Empty;
}
