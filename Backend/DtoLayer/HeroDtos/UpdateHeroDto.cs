using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.HeroDtos;

public class UpdateHeroDto
{
    //public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string FullName { get; set; } = string.Empty;
    public string ScrollingText { get; set; } = string.Empty;
    public string HeroAbout { get; set; } = string.Empty;
}
