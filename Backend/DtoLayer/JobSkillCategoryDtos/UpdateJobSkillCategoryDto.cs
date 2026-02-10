using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillCategoryDtos;

public class UpdateJobSkillCategoryDto
{
    public Guid Id { get; set; }
    public string CategoryDescription { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
}
