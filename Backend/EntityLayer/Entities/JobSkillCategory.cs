namespace CV.EntityLayer.Entities;

public sealed class JobSkillCategory:BaseEntity
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;

    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>(); //kategori listesi frontend altında react javascript gibi
}
