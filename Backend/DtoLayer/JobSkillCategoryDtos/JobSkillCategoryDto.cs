using DtoLayer.JobSkillsDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillCategoryDtos;

public class JobSkillCategoryDto
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;

    public List<JobSkillDto> JobSkills { get; set; } = new(); //Bu kategorideki yetenekler  Kart içindeki liste: React %90, Vue %85...
}
