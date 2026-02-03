using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.EducationDtos;

public class CreateEducationDto
{
    public string EducationDegree { get; set; } = string.Empty;
    public DateTime EducationStartDate { get; set; }
    public DateTime? EducationFinishDate { get; set; }
    public string EducationSchoolName { get; set; } = string.Empty;
    public string EducationDescription { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
