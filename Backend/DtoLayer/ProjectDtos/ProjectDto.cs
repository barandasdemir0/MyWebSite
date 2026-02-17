namespace DtoLayer.ProjectDtos;

public class ProjectDto
{
    public Guid Id { get; set; }
    // Temel
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Bilgiler
    public string? ClientName { get; set; }
    public string? Duration { get; set; }
    public string? Role { get; set; }

    // Detaylar
    public string? Goals { get; set; }

    // Linkler & Tech
    public string? WebsiteUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    // İlişkiler (Okuma)

    public List<string> RelatedBlogs { get; set; } = new();  // Blog başlıkları
    public List<Guid> TopicIds { get; set; } = new();
    public List<string> Topics { get; set; } = new();        // Topic isimleri

}
