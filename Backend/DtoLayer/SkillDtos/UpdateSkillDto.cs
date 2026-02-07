using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.SkillDtos;

public class UpdateSkillDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string SkillName { get; set; } = string.Empty;
    public string SkillUrl { get; set; } = string.Empty;
    public string SkillIcon { get; set; } = string.Empty;
    public int Percentage { get; set; }
    public int DisplayOrder { get; set; }
}
