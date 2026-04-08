using SharedKernel.Shared;

namespace DtoLayer.JobSkillsDtos;

public class UpdateJobSkillDto : IHasId
{
    public Guid Id { get; set; }
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentage{ get; set; }
    public Guid JobSkillCategoryId { get; set; }
}
