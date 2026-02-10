using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.SkillDtos;

public class SkillDto
{
    public Guid Id { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string IconifyIcon { get; set; } = string.Empty;

}
