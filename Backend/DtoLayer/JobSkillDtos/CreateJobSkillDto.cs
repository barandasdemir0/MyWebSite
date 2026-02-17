namespace DtoLayer.JobSkillsDtos;

public class CreateJobSkillDto
{
    public string JobSkillName { get; set; } = string.Empty;
    public int JobSkillPercentage{ get; set; }
    public Guid JobSkillCategoryId { get; set; } 
}
