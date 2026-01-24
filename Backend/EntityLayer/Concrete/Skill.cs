using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Skill : BaseEntity
    {
        public string SkillName { get; set; } = string.Empty;
        public string SkillUrl { get; set; } = string.Empty;
        public string SkillIcon { get; set; } = string.Empty;
        public int Percentage { get; set; }
        public int DisplayOrder { get; set; }

 
    }
}
