using DtoLayer.JobSkillCategoryDtos;
using DtoLayer.JobSkillsDtos;

namespace WebUILayer.Areas.Admin.Models;

public class JobSkillUpdateViewModel
{
    public UpdateJobSkillDto jobSkillDtos { get; set; } = new();
    public JobSkillCategoryDto? jobSkillCategoryDtos { get; set; }
}
