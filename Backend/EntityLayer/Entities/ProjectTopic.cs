namespace CV.EntityLayer.Entities;

public sealed class ProjectTopic
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid TopicId { get; set; }
    public Topic Topic { get; set; } = null!;
}
