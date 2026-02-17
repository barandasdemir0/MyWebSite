using DtoLayer.JobSkillCategoryDtos;
using DtoLayer.JobSkillsDtos;

namespace WebUILayer.Areas.Admin.Models;

public class JobSkillCreateViewModel
{
    public CreateJobSkillDto jobSkillDtos { get; set; } = new();
    public JobSkillCategoryDto? jobSkillCategoryDtos { get; set; }
}
