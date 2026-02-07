using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ExperienceDtos;

public class CreateExperienceDto
{
    public string ExperienceTitle { get; set; } = string.Empty;
    public DateTime? ExperienceStartDate { get; set; }
    public DateTime? ExperienceFinishDate { get; set; }
    public string ExperienceCompanyName { get; set; } = string.Empty;
    public string ExperienceDescription { get; set; } = string.Empty;
    public int? DisplayOrder { get; set; }   // Sıralama için
}
