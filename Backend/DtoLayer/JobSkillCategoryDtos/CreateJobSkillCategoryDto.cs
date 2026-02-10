using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillCategoryDtos;

public class CreateJobSkillCategoryDto
{
    public string CategoryDescription { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
}
