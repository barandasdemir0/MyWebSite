namespace DtoLayer.ProjectDtos;

public class CreateProjectDto
{
    // Temel alanların hepsi (Id hariç)
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ClientName { get; set; }
    public string? Duration { get; set; }
    public string? Role { get; set; }
    public string? Goals { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;
    public bool IsPublished { get; set; } = false;

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();
}
