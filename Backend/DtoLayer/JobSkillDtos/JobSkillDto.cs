using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.JobSkillsDtos;

public class JobSkillDto
{
    public Guid Id { get; set; }
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentage{ get; set; }
    public bool IsDeleted { get; set; } = false;

    public Guid JobSkillCategoryId { get; set; } // Category ID (hidden / backend için) Güncelleme formunda dropdown'da seçili kategori için
    public string CategoryName { get; set; } = string.Empty;  // UI'da gösterilecek isim Tabloda göstermek için: "Frontend", "Backend"



}
