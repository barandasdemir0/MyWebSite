using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.SkillDto
{
    public class SkillDto
    {
        public Guid Id { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string SkillUrl { get; set; } = string.Empty;
        public string SkillIcon { get; set; } = string.Empty;
        public int Percentage { get; set; }
        public int DisplayOrder { get; set; }
    }
}
