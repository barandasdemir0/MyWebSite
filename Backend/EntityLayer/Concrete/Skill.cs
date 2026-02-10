using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Skill : BaseEntity
    {
        public string SkillName { get; set; } = string.Empty;
        public string IconifyIcon { get; set; } = string.Empty;
 
    }
}
