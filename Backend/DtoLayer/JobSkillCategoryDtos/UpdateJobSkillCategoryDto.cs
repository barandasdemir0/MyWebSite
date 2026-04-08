using SharedKernel.Shared;

namespace DtoLayer.JobSkillCategoryDtos;

public class UpdateJobSkillCategoryDto : IHasId
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
}
