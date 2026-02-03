using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.AboutDtos;

public class UpdateAboutDto
{
    //public Guid Id { get; set; }  // Hangi kayıt güncelleniyor  --> kaldırdık çünkü updatte ıd alanı güncellenmez zaten biz entityde protected alanı verdik
    public string FullName { get; set; } = string.Empty;
    public string Greeting { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public int ProjectCount { get; set; }
    public int ExperienceYear { get; set; }
    public int ProjectDrink { get; set; }
}
