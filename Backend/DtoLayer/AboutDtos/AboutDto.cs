using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.AboutDtos;

public class AboutDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Greeting { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public int ProjectCount { get; set; }
    public int ExperienceYear { get; set; }
    public int ProjectDrink { get; set; }
}
