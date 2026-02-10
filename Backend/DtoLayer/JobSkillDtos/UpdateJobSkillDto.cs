using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillsDtos;

public class UpdateJobSkillDto
{
    public Guid Id { get; set; }
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentange { get; set; }
    public Guid JobSkillCategoryId { get; set; }
}
