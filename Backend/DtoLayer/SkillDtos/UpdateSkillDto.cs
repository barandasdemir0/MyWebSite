using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.SkillDtos;

public class UpdateSkillDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string SkillName { get; set; } = string.Empty;
    public string IconifyIcon { get; set; } = string.Empty;
}
