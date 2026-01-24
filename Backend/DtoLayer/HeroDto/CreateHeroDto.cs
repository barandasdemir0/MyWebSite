using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.HeroDto
{
    public class CreateHeroDto
    {
        public string FullName { get; set; } = string.Empty;
        public string ScrollingText { get; set; } = string.Empty;
        public string HeroAbout { get; set; } = string.Empty;
    }
}
