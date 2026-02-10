using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Concrete
{
    public sealed class JobSkill:BaseEntity
    {
        public string JobSkillName { get; set; } = string.Empty;
        // FK
        public Guid JobSkillCategoryId { get; set; }

        // Navigation
        public JobSkillCategory JobSkillCategory { get; set; } = null!;
    }
}
