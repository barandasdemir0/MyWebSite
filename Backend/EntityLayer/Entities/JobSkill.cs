namespace CV.EntityLayer.Entities;

public sealed class JobSkill:BaseEntity
{
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentage{ get; set; }
    // FK
    public Guid JobSkillCategoryId { get; set; }
    //Frontend kategorisinin Id'si

    // Navigation 
    public JobSkillCategory JobSkillCategory { get; set; } = null!;
    //Frontend kategorisi objesi yani bu yetenek hangi kategoriye ait

}
