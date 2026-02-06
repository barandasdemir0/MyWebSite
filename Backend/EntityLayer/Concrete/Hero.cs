using System;
using System.Collections.Generic;
using System.Text;

namespace CV.EntityLayer.Entities
{
    public sealed class Hero:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfessionalTitle { get; set; } = string.Empty;
        public string ScrollingText { get; set; } = string.Empty;
        public string HeroAbout { get; set; } = string.Empty;
    }
}
