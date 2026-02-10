using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillsDtos;

public class CreateJobSkillDto
{
    public string JobSkillName { get; set; } = string.Empty;
    public Guid JobSkillCategoryId { get; set; }
}
