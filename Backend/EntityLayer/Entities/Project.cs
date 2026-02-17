namespace CV.EntityLayer.Entities;

public sealed class Project : BaseEntity
{
    // Temel Bilgiler
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Proje Bilgileri
    public string? ClientName { get; set; }
    public string? Duration { get; set; }
    public string? Role { get; set; }

    // Detaylı Açıklamalar
    public string? Goals { get; set; }

    // Linkler
    public string? WebsiteUrl { get; set; }
    public string? GithubUrl { get; set; }

    // Teknolojiler (virgülle ayrılmış)
    public string Technologies { get; set; } = string.Empty;

    // Diğer
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    // Many-to-Many Navigation
    public ICollection<ProjectTopic> ProjectTopics { get; set; } = new List<ProjectTopic>();
}
