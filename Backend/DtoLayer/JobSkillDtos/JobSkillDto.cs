using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillsDtos;

public class JobSkillDto
{
    public Guid Id { get; set; }
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentange { get; set; }

    public Guid JobSkillCategoryId { get; set; }// Category ID (hidden / backend için)
    public string CategoryName { get; set; } = string.Empty;  // UI'da gösterilecek isim
    public bool IsDeleted { get; set; } = false;


}
